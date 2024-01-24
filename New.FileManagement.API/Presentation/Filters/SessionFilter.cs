using Application.Common.Constants.ErrorBuldles;
using Application.Common.Models;
using GlobalPay.FileSystemManager.Application.Common.DTOs;
 
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Net;

namespace GlobalPay.FileSystemManager.Presentation.Filters
{
    public class SessionFilter : IAsyncActionFilter
    {
        private readonly IMessageProvider _messageProvider;
        private readonly ILogger<SessionFilter> _logger;
        private readonly IConfiguration _config;
        private readonly AllowableActionmethods _allowedActionmethodsWithoutAuthorization;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _language;
        public SessionFilter(IMessageProvider messageProvider, ILogger<SessionFilter> logger, IConfiguration config, IOptions<AllowableActionmethods> opt, IHttpContextAccessor httpContext)
        {
            _messageProvider = messageProvider ?? throw new ArgumentNullException(nameof(messageProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _language = _httpContext.HttpContext.Request.Headers[ResponseCodes.LANGUAGE];
            _allowedActionmethodsWithoutAuthorization = opt.Value;
        }
 
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            bool hasAppId = context.HttpContext.Request.Headers.TryGetValue("appid", out var appId);

          
           if (hasAppId && !string.IsNullOrWhiteSpace(appId) && !string.IsNullOrEmpty(appId))
               
            
            {
                var myAppId = _config.GetValue<string>("SystemSettings:AppId");
                if (myAppId != null && myAppId.Equals(appId))

                {

                    await next();
                }
                else
                {
                    context.Result = new ObjectResult(
                                 new ErrorResponse<dynamic>
                                 {
                                     responseCode = ResponseCodes.INVALID_APPID,
                                     responseDescription = _messageProvider.GetMessage(ResponseCodes.INVALID_APPID, "en")
                                 })
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                    _logger.LogInformation("Invalid Api ID");
                    return;

                }
            }
            else
            {
                context.Result = new ObjectResult(
              new ErrorResponse<dynamic>
              {
                  responseCode = ResponseCodes.INVALID_APPID,
                  responseDescription = _messageProvider.GetMessage(ResponseCodes.INVALID_APPID, "en")
              })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                _logger.LogInformation("Invalid INVALID_API_ID");
                return;
            }





        }

    }
}

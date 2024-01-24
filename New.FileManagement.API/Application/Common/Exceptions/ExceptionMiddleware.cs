

using Microsoft.AspNetCore.Builder;

namespace Application.Common.Exceptions
{
    public static class ExceptionMiddleware
    {
        public static void UseApiExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory, IMessageProvider messageProvider)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var getLanguage = Convert.ToString(context.Response.Headers["language"]);
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    //if any exception then report it and log it
                    if (contextFeature != null)
                    {
                        //Technical Exception for troubleshooting
                        var logger = loggerFactory.CreateLogger("GlobalException");
                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                        //Business exception - exit gracefully
                        await context.Response.WriteAsync(
                            messageProvider.GetMessage(getLanguage).ToString());
                    }
                });
            });
        }
    }
}

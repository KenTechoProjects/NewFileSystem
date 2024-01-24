using Application.Common.Constants.ErrorBuldles;
using GlobalPay.FileSystemManager.Application.Common.DTOs;
using GlobalPay.FileSystemManager.Application.Interfacses.FileSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalPay.FileSystemManager.Application.Helpers
{
  

    public class AzureCloudMailHelper : IAzureCloudMailHelper
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _config;
        private readonly string _httpClientName = string.Empty;
        private readonly string _baseUrl = string.Empty;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _language = string.Empty;

        // private readonly AzureEmailConfiguration _azureEmailConfiguration;
        public AzureCloudMailHelper(ILoggerFactory logger, IHttpClientFactory httpClient, IConfiguration config, IMessageProvider message, IHttpContextAccessor httpContext)
        {
            //_azureEmailConfiguration = azureEmailConfiguration ?? throw new ArgumentNullException(nameof(azureEmailConfiguration));
            _logger = logger.CreateLogger<AzureCloudMailHelper>();
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _config = config;
            _httpContext = httpContext;

        }

        public async Task<ServerResponses<bool>> PostMessageAsync(AzureEmailCloudModel request)
        {
            var response = new ServerResponses<bool>();
            using HttpClient client = _httpClient.CreateClient(_httpClientName ?? "");

            client.DefaultRequestHeaders.Add("language", "en");
            var url = _config.GetValue<string>("GlobalEmailService:Url");

            var input = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"REQUEST:", input);
            var message = new StringContent(input, Encoding.UTF8, "application/json");

            var rawResponse = await client.PostAsync(url, message);
            var body = await rawResponse.Content.ReadAsStringAsync();
            if (rawResponse.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Messaging system  mail Response : {body}");

                // _logger.LogInformation("  PostMessage Response {0}", JsonConvert.SerializeObject(body));

                if (body != null)
                {
                    var data = JsonConvert.DeserializeObject<ServerResponses<bool>>(body);
                    response = data;
                    response.IsSuccessful = true;

                }
                else
                {
                    response.Error = new ErrorResponse { ResponseCode = "00000000090", ResponseDescription = "Something went wrong" };
                }
            }
            else
            {
                response.Error = new ErrorResponse { ResponseCode = "00000000090", ResponseDescription = "Something went wrong" };
            }


            return response;

        }
    }
}

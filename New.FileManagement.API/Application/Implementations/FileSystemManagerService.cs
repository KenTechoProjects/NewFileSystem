using Application.Common.Constants.ErrorBuldles;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GlobalPay.FileSystemManager.Application.Common.DTOs;
using GlobalPay.FileSystemManager.Application.Interfacses.FileSystems;
using Microsoft.Extensions.Hosting;
//using Azure.Storage.Blobs;
//using Azure.Storage.Blobs.Models;
namespace GlobalPay.FileSystemManager.Application.Implementations
{

    public class FileSystemManagerService : IFileSystemManagerService
    {

        private readonly IHostEnvironment _hostEnvironment;

        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        private readonly string _videoLocalPath = string.Empty;
        private readonly string _otherFiles = string.Empty;
        private readonly string _videoWebFileURL = string.Empty;
        private readonly string _otherWebFileURL = string.Empty;
        private readonly IAzureCloudMailHelper _azureCloudMailHelper;
        private readonly string _baseUrl = string.Empty;
        public FileSystemManagerService(IHostEnvironment hostEnvironment, ILoggerFactory logger, IConfiguration config, IAzureCloudMailHelper azureCloudMailHelper)
        {
            _azureCloudMailHelper = azureCloudMailHelper;

            _hostEnvironment = hostEnvironment;

            _logger = logger.CreateLogger<FileSystemManagerService>();
            _config = config;



            _baseUrl = _config.GetValue<string>("SystemSettings:BaseUrl");
            _azureCloudMailHelper = azureCloudMailHelper;
        }

        public async Task<ServerResponse<string>> SaveToAzureBlob(IFormFile file, string blobName)
        {
            var response = new ServerResponse<string>();
            var connectionString = _config.GetSection("AzureBlobConnectionSettings").GetValue<string>("DefaultConnection");
            var containerName = _config.GetSection("AzureBlobConnectionSettings").GetValue<string>("ContainerName");

            // Create a BlobServiceClient object using the connection string.
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Get a reference to the container.
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // check if blob already exist

            await containerClient.DeleteBlobIfExistsAsync(blobName);


            // Get a reference to the blob with the specified name.
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Upload the file to Azure Blob Storage.
            using (Stream stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
                });
            }
            //Console.WriteLine($"File uploaded successfully to Azure Blob Storage. URI: {blobClient.Uri}");

            response.Data = blobClient.Uri.ToString();
            response.IsSuccessful = true;
            return response;
        }

        public string GetFileByConfigOnBase64(string filename)
        {
            if (!string.IsNullOrWhiteSpace(filename))
            {

                var returnedBaseUrl = string.Empty;
                string[] fileExt = filename.Split('.');
                if ("wav,jpg,jpeg,png".Contains(fileExt[1]))
                {
                    returnedBaseUrl = $"{_baseUrl}/OtherFiles/";
                }
                else
                {
                    returnedBaseUrl = $"{_baseUrl}/VideoFiles/";
                }
                if (returnedBaseUrl != null)
                {
                    var fileWithPath = $"{returnedBaseUrl}{filename}";
                    string base64 = ConvertImageURLToBase64(fileWithPath);
                    return base64;


                }
            }
            return string.Empty;
        }
        public string GetFileByUrl(string filename)
        {
            if (!string.IsNullOrWhiteSpace(filename))
            {
                string[] fileExt = filename.Split('.');
                var returnedBaseUrl = string.Empty;
                if ("wav,jpg,jpeg,png".Contains(fileExt[1]))
                {
                    returnedBaseUrl = $"{_baseUrl}/OtherFiles/";
                }
                else
                {
                    returnedBaseUrl = $"{_baseUrl}/VideoFiles/";
                }
                if (returnedBaseUrl != null)
                {
                    var fileWithPath = $"{returnedBaseUrl}{filename}";

                    return fileWithPath;


                }
            }
            return string.Empty;
        }





        public string LocalLocation { get; set; }
        public async Task<ServerResponse<SaveImageResponse>> SaveFilesWebAPIFolder(string rawFile, string fileExtension, string fileName = "")
        {
            var response = new ServerResponse<SaveImageResponse>();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = Guid.NewGuid().ToString();
            }


            var filePath = $"{_hostEnvironment.ContentRootPath}{Path.DirectorySeparatorChar}{Path.DirectorySeparatorChar}FileContents{Path.DirectorySeparatorChar}OtherFiles{Path.DirectorySeparatorChar}";

            if (fileExtension?.ToLower().Contains(".mp4") == true || fileExtension?.ToLower().Contains(".wav") == true || fileExtension?.ToLower().Contains(".mp4") == true)
            {

                filePath = $"{_hostEnvironment.ContentRootPath}{Path.DirectorySeparatorChar}FileContents{Path.DirectorySeparatorChar}VideoFiles{Path.DirectorySeparatorChar}";
            }

            if (string.IsNullOrEmpty(rawFile))
            {
                return response;
            }
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            fileName = fileName.Replace(" ", "_");
            var filePaths = $"{filePath}{fileName}{fileExtension}";
            if (File.Exists($"{filePaths}"))
            {
                File.Delete($"{filePaths}");
            }



            await File.WriteAllBytesAsync(filePaths, Convert.FromBase64String(rawFile));

            LocalLocation = filePaths;
            response.SuccessMessage = "Saved";
            response.IsSuccessful = true;
            response.Data = new SaveImageResponse
            {
                FileName = $"{fileName}{fileExtension}",
                FilePath = filePaths

            };
            return response;

        }



        public async Task<ServerResponses<SaveImageResponse>> SaveFilesWebAPIFolderFromFile(CreateFileDTO request)
        {
            var response = new ServerResponses<SaveImageResponse>();
            string fileName = string.Empty;
            if (!request.ISValid(out string error))
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.INVALID_Parameter,
                    ResponseDescription = error
                };
                return response;
            }
            string fName = Path.GetFileNameWithoutExtension(request.File.FileName);
          
            if (request.FileCategory == Common.Enums.FileCategory.Logo)
            {
                fileName = $"{request.MerchantName}_{fName}";
            }
            else
            {
                fileName = $"{request.MerchantName}{Guid.NewGuid().ToString("N")}";
            }

            fileName = fileName.Replace(" ","_");


            var filePath = $"{_hostEnvironment.ContentRootPath}{Path.DirectorySeparatorChar}FileContents{Path.DirectorySeparatorChar}OtherFiles{Path.DirectorySeparatorChar}";


            string fileExtension = Path.GetExtension(request.File.FileName);
            if (fileExtension?.ToLower().Contains(".mp4") == true || fileExtension?.ToLower().Contains(".wav") == true || fileExtension?.ToLower().Contains(".mp4") == true)
            {

                filePath = $"{_hostEnvironment.ContentRootPath}{Path.DirectorySeparatorChar}{Path.DirectorySeparatorChar}FileContents{Path.DirectorySeparatorChar}VideoFiles{Path.DirectorySeparatorChar}";

            }


            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }


            var filePaths = $"{filePath}{fileName}{fileExtension}";
            if (File.Exists($"{filePaths}"))
            {
                File.Delete($"{filePaths}");
            }



            using (var stream = new FileStream(filePaths, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
                LocalLocation = filePaths;
                response.SuccessMessage = "Saved";
                response.IsSuccessful = true;
                response.Data = new SaveImageResponse
                {
                    FileName = $"{fileName}{fileExtension}",
                    FilePath = filePaths

                };


                return response;
            }




        }


        public async Task<bool> TestMailImage(string filenameWithExtension)
        {


            if (string.IsNullOrEmpty(filenameWithExtension))
            {
                return false;
            }
            var mailPath = $"{_hostEnvironment.ContentRootPath}{Path.DirectorySeparatorChar}EmailTemplate{Path.DirectorySeparatorChar}TestImageDisplayOnMail.html";


            string imageUrl = string.Empty;
            string[] fileExt = filenameWithExtension.Split('.');
            var returnedBaseUrl = string.Empty;
            if ("wav,jpg,jpeg,png".Contains(fileExt[1]))
            {
                returnedBaseUrl = $"{_baseUrl}OtherFiles/";
            }
            else
            {
                returnedBaseUrl = $"{_baseUrl}VideoFiles/";
            }

            imageUrl = $"{returnedBaseUrl}{filenameWithExtension}";


            StreamReader str = new StreamReader(mailPath);
            string mailText = str.ReadToEnd();
            str.Close();


            mailText = mailText
                .Replace("{{BUSINESSNAME}}", "Test")
                .Replace("{{IMAGE}}", imageUrl);


            var manageAccountTeam = new List<To>();
            var reciver = _config.GetValue<string>("GlobalEmailService:Receivers").Split(",");
 




            foreach (var item in reciver)
            {
                manageAccountTeam.Add(new To { email = item, name = "Globalpay Team Member".ToUpper() });
            }
            var payload = new AzureEmailCloudModel
            {
                content = new List<Content> { new Content { type = "html", value = mailText } },
                from = new From { email = "GlobalpayTest@zenithbank.com", name = "GlobalPay" },
                subject = "testing Image Display",
                personalizations = new List<Personalization>
   {
       new Personalization { to=manageAccountTeam  } }

            };
            var send = await _azureCloudMailHelper.PostMessageAsync(payload);


            if (send != null && send.IsSuccessful)
            {
                return true;
            }

            return false;



        }

        public String ConvertImageURLToBase64(String url)
        {

            StringBuilder _sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(url))
            {
                Byte[] _byte = this.GetFile(url, true);

                if (_byte != null) { _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length)); }
                //_sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
            }


            return _sb.ToString();
        }
        public string GetImage(string photoUrl)
        {
            _logger.LogInformation("Inside the GetImage  method of the DocumetReviewService");
            _logger.LogInformation($"Document URL={photoUrl} passed to the GetImage method of the DocumetReviewService");

            var finaleImagePath = string.Empty;
            var base64Image = string.Empty;
            string returnedBaseUrl = string.Empty;

            if (!string.IsNullOrWhiteSpace(photoUrl))
            {
                var photoLocationSplit = photoUrl.Split('\\');


                var photoImage = photoLocationSplit[photoLocationSplit.Length - 1];
                string getphoneNumber = photoImage.Split('_')[0];
                string imageWithPath = $"{returnedBaseUrl}{getphoneNumber}/{photoImage}";// Path.Combine(returnedBaseUrl, photoImage);
                                                                                         //string imageWithPath = $"{returnedBaseUrl}221771000000.jpg";

                finaleImagePath = imageWithPath.Replace('\\', '/');
                base64Image = ConvertImageURLToBase64(finaleImagePath);
                if (string.IsNullOrWhiteSpace(base64Image))
                {
                    _logger.LogInformation("ConvertImageURLToBase64  returned an empty base64string");
                }
                else
                {
                    _logger.LogInformation("ConvertImageURLToBase64  returned  value");
                }
            }


            return base64Image;

        }

        private byte[] GetFile(string url, bool isUrl)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                WebProxy myProxy = new WebProxy();
                if (!string.IsNullOrWhiteSpace(url))
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                    #region Added newly 30-10-2021

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, cert, chain, sslPolicyErrors) => true;
                    req.Credentials = CredentialCache.DefaultCredentials;
                    req.Method = "GET";
                    req.ContentType = "application/x-www-form-urlencoded";

                    #endregion

                    HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                    stream = response.GetResponseStream();

                    using (BinaryReader br = new BinaryReader(stream))
                    {
                        int len = (int)(response.ContentLength);
                        buf = br.ReadBytes(len);
                        br.Close();
                    }

                    stream.Close();
                    response.Close();

                    return buf;
                }

            }
            catch (Exception exp)
            {
                //  errorOcurred = true;

                buf = null;
            }

            return null;

        }


    }

}

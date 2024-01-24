using GlobalPay.FileSystemManager.Application.Common.DTOs;
using GlobalPay.FileSystemManager.Application.Common.Enums;
using GlobalPay.FileSystemManager.Application.Interfacses.FileSystems;
using Mapster;

namespace GlobalPay.FileSystemManager.Presentation.Areas.FileManagement
{
    [Area("FileManagement")]
    [Route("api/filemanagement")]
    //[Authorize]
    public class ImageManagementController : BaseController
    {

        private readonly IFileSystemManagerService _fileSystemManager;

        public ImageManagementController(IFileSystemManagerService fileSystemManager)
        {
            _fileSystemManager = fileSystemManager;
        }
        [HttpPost("upload")]
      
        public async Task<IActionResult> UploadFile([FromBody] FileRequestsDTO request)
        {
            var result = await _fileSystemManager.SaveFilesWebAPIFolder(request.RawFile, request.FileExtension, request.FileName);
            return Ok(result);
        }
        [HttpPost("uploadfile")]
        [Produces("application/json")]
        public async Task<IActionResult> UploadFile(FileCategory category,string merchantName, IFormFile file)
        {
            var model = new CreateFileDTO {  File=file, FileCategory=category,  MerchantName=merchantName};
            model.File = file;
            var result = await _fileSystemManager.SaveFilesWebAPIFolderFromFile(model);
            return Ok(result);
        }
        // Uploading to Azure blob
        [HttpPost("upload-To-Azure-Blob")]
       
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadFileToAzureBlob(IFormFile file, string blobName)
        {
            var result = await _fileSystemManager.SaveToAzureBlob(file, blobName);

            return Ok(result);
        }


        [HttpGet("display/file/base64/{fileNameWithExtension}")]
       
        public IActionResult GetFileByBase64String([FromRoute]string fileNameWithExtension)
        {
            var result = _fileSystemManager.GetFileByConfigOnBase64(fileNameWithExtension);
            return Ok(result);
        }


        [HttpGet("display/file/url/{fileNameWithExtension}")]
        
        public IActionResult GetFileByUrl([FromRoute]string fileNameWithExtension)
        {
            var result = _fileSystemManager.GetFileByUrl(fileNameWithExtension);
            return Ok(result);
        } 
        [HttpGet("test/file/{fileNameWithExtension}")]
        public async Task<IActionResult> TestMailImage([FromRoute]string fileNameWithExtension)
        {
            var result =await _fileSystemManager.TestMailImage(fileNameWithExtension);
            return Ok(result);
        }


    }

}

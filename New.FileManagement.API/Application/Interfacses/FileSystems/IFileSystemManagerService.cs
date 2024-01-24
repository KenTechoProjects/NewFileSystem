using GlobalPay.FileSystemManager.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalPay.FileSystemManager.Application.Interfacses.FileSystems
{
    public interface IFileSystemManagerService
    {


        Task<ServerResponse<SaveImageResponse>> SaveFilesWebAPIFolder(string rawFile, string fileExtension, string fileName = "");
        string LocalLocation { get; set; }

        //string GetVideoByConfigOnBase64(string filename);
        string GetFileByConfigOnBase64(string filename);
        string GetFileByUrl(string filename);
        //string GetVideoByUrl(string filename);
        Task<ServerResponse<string>> SaveToAzureBlob(IFormFile file, string blobName);
        Task<ServerResponses<SaveImageResponse>> SaveFilesWebAPIFolderFromFile(CreateFileDTO request);
        Task<bool> TestMailImage(string filenameWithExtension);
    }

}

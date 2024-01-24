using GlobalPay.FileSystemManager.Application.Common.Enums;

namespace GlobalPay.FileSystemManager.Application.Common.DTOs
{
    public class CreateFileDTO
    {
        public IFormFile File { get; set; }
        public string MerchantName { get; set; }
        public FileCategory FileCategory { get; set; }

        public bool ISValid(out string err)
        {
            if (File is null)
            {
                err = "File can no tbe empty";
                return false;
            }
            if (File.Length <= 0)
            {
                err = "File can no tbe empty";
                return false;
            }
            if (string.IsNullOrEmpty(MerchantName))
            {
                err = "MerchantName is required";
                return false;
            }
            err = string.Empty;
            return true;
        }
    }
public class CreateFileDTOV2
    {
     
        public string MerchantName { get; set; }
        public FileCategory FileCategory { get; set; }

        public bool ISValid(out string err)
        {
            
            if (string.IsNullOrEmpty(MerchantName))
            {
                err = "MerchantName is required";
                return false;
            }
            err = string.Empty;
            return true;
        }
    }

}

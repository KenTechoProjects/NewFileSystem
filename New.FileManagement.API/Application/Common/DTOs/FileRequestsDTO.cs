using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalPay.FileSystemManager.Application.Common.DTOs
{
    public class FileRequestsDTO
    {
        public string RawFile { get; set; }
        public string FileExtension { get; set; }
        public string FileName { get; set; }
    }
}

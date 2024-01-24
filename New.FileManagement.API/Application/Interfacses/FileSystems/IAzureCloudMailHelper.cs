using GlobalPay.FileSystemManager.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalPay.FileSystemManager.Application.Interfacses.FileSystems
{

    public interface IAzureCloudMailHelper
    {
        Task<ServerResponses<bool>> PostMessageAsync(AzureEmailCloudModel request);
    }
}

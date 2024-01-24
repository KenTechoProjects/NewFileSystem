using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalPay.FileSystemManager.Application.Common.DTOs
{
    public class AzureEmailCloudModel
    {
        public From from { get; set; }
        public string subject { get; set; }
        public List<Content> content { get; set; }
        public List<Personalization> personalizations { get; set; }

    }


    public class Content
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class From
    {
        public string email { get; set; }
        public string name { get; set; }
    }

    public class Personalization
    {
        public List<To> to { get; set; }
    }
    public class To
    {
        public string email { get; set; }
        public string name { get; set; }
    }
}

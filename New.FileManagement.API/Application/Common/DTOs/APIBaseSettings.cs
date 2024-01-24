using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.DTOs
{
    public  class APIBaseSettings
    {
        public string BaseUrl { get; set; }
        public string GlobalPayBaseUrl { get; set; }
        public string QuerySingleTransaction { get; set; }

        public string UnitUrl { get; set; }

        public string appid { get; set; }
        public string language { get; set; }
        public Double timeoutval { get; set; }

        public string Dumyapi {  get; set; }





    }
}

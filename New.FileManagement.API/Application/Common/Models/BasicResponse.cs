

using System.ComponentModel;

namespace Application.Common.Models
{
     
    public class BasicResponse<T>
    {
        [DefaultValue(true)]
        public bool IsSuccessful { get; set; }
        [DefaultValue(null)]
        public ErrorResponse<T> Error { get; set; }

        public BasicResponse()
        {
            IsSuccessful = false;
        }
        public BasicResponse(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
      


    }
public class BasicResponse
    {
        [DefaultValue(true)]
        public bool IsSuccessful { get; set; }
        [DefaultValue(null)]
        public ErrorResponse Error { get; set; }

        public BasicResponse()
        {
            IsSuccessful = false;
        }
        public BasicResponse(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
      


    }
}


namespace Application.Common.Models
{ public class ServerResponse<T> : BasicResponse<T>
    {
        public ServerResponse(bool success=false)
        {
            IsSuccessful=success;
        }
        public T Data { get; set; }
        public string SuccessMessage { get; set; }
        public string ResponseCode { get; set; }
         
    }
   // public class ServerResponse<T> : BasicResponse<T>
    public class ServerResponses<T> : BasicResponse
    {
        public ServerResponses(bool success=false)
        {
            IsSuccessful=success;
        }
        public T Data { get; set; }
        public string SuccessMessage { get; set; }
        public string ResponseCode { get; set; }
    }
}

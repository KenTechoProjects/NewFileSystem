
namespace Application.Common.Models
{
    public class ErrorResponse<T>
    {
        public string responseCode { get; set; }
        public string responseDescription { get; set; }
        public T Data { get; set; }


        public static T Create<T>(string errorCode, string errorMessage) where T : BasicResponse<T>, new()
        {
            var response = new T
            {
                IsSuccessful = false,
                Error = new ErrorResponse<T>
                {
                    responseCode = errorCode,
                    responseDescription = errorMessage
                }
            };
            return response;
        }

        public override string ToString()
        {
            return $"{responseCode} :-: {responseDescription}";
        }

    }


 public class ErrorResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }

        



        public static T Create<T>(string errorCode, string errorMessage) where T : BasicResponse, new()
        {
            var response = new T
            {
                IsSuccessful = false,
                Error = new ErrorResponse
                {
                    ResponseCode = errorCode,
                    ResponseDescription = errorMessage
                }
            };
            return response;
        }

        public override string ToString()
        {
            return $"{ResponseCode} :-: {ResponseDescription}";
        }

    }


}

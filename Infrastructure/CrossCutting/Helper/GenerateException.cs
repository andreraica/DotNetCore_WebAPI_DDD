using System;
using System.Net;
using Newtonsoft.Json;

namespace Infrastructure.CrossCutting.Helper
{
    public static class GenerateException
    {
        public class ExceptionParameter
        {
            public HttpStatusCode StatusCode { get; set; }
            public string Output { get; set; }
            public object Input { get; set; }
            public string Url { get; set; }
        }

        public static Exception Throw(string friendlyMessage, ExceptionParameter parameter)
        {
           return new Exception(friendlyMessage, 
                    new Exception(string.Format("StatusCode: {0}, Url: {1}, Input: {2}, Output: {3}",
                            (int)parameter.StatusCode,
                            parameter.Url,
                            JsonConvert.SerializeObject(parameter.Input),
                            parameter.Output)
                    )
           ); 
        }

        public static Exception Throw(string friendlyMessage, Exception innerException)
        {
           return new Exception(friendlyMessage, innerException);
        }

        public static UnauthorizedAccessException ThrowUnauthorizedException(string friendlyMessage)
        {
           return new UnauthorizedAccessException(friendlyMessage);
        }
        

        
    }
}

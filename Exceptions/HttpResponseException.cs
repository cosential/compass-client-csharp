using System;
using System.Runtime.Serialization;
using RestSharp;

namespace Cosential.Integrations.Compass.Client
{
    [Serializable]
    public class HttpResponseException : Exception
    {
        public HttpResponseException()
        {
        }

        public HttpResponseException(string message) : base(message)
        {
            Message = message;
        }

        public HttpResponseException(IRestResponse response) : base(string.Empty, response?.ErrorException)
        {
            Message = response != null ? $"{response.Request.Method} to [{response.ResponseUri}] resulted in error" : "A response was not returned and the method and uri where not captured.";
        }

        public HttpResponseException(string message, Exception innerException) : base(message, innerException)
        {
            Message = message;
        }

        protected HttpResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message { get; }
        
    }
}
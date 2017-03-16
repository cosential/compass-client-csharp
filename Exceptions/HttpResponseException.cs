using System;
using System.Runtime.Serialization;
using RestSharp;

namespace Cosential.Integrations.Compass.Client
{
    [Serializable]
    internal class HttpResponseException : Exception
    {
        private IRestResponse response;

        public HttpResponseException()
        {
        }

        public HttpResponseException(string message) : base(message)
        {
        }

        public HttpResponseException(IRestResponse response)
        {
            this.response = response;
        }

        public HttpResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
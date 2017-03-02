using System;
using System.Net;
using System.Runtime.Serialization;
using RestSharp;

namespace Cosential.Integrations.Compass.Client
{
    [Serializable]
    internal class ResponseStatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ResponseContent { get; }
        public ResponseStatusCodeException()
        {
        }

        public ResponseStatusCodeException(IRestResponse response) : base($"Web service responded with status code: {response.StatusCode:D}. Content: {response.Content}", response.ErrorException)
        {
            StatusCode = response.StatusCode;
            ResponseContent = response.Content;
        }
    }
}
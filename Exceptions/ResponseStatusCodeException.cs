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

        public ResponseStatusCodeException(IRestResponse response) : base($"Web service [{response.ResponseUri}] responded with status code: [{response.StatusCode:D}]", response.ErrorException)
        {
            StatusCode = response.StatusCode;
            ResponseContent = response.Content;
        }
    }
}
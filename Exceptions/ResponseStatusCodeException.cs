using System;
using System.Linq;
using System.Net;
using System.Text;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Exceptions
{
    [Serializable]
    internal class ResponseStatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ResponseContent { get; }
        public ResponseStatusCodeException()
        {
        }

        public ResponseStatusCodeException(IRestResponse response) : base(BuildCurl(response), response.ErrorException)
        {
            StatusCode = response.StatusCode;
            ResponseContent = response.Content;
        }

        public static string BuildCurl(IRestResponse response)
        {
            var sb = new StringBuilder();

            sb.Append($"**API REQUEST FAILED**:\ncurl -X {response.Request.Method}");

            foreach (var h in response.Request.Parameters)
            {
                switch (h.Type)
                {
                    case ParameterType.HttpHeader:
                        sb.Append($" -H \"{h.Name}: {h.Value}\"");
                        break;
                    case ParameterType.RequestBody:
                        sb.Append($" -d '{h.Value}'".Replace(Environment.NewLine, ""));
                        break;
                    case ParameterType.QueryString:
                        break;
                    case ParameterType.Cookie:
                        break;
                    case ParameterType.GetOrPost:
                        break;
                    case ParameterType.UrlSegment:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            sb.Append($" \"{response.ResponseUri}\"");

            return sb.ToString();
        }
    }
}
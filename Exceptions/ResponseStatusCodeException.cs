using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
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

        public ResponseStatusCodeException(IRestResponse response, IRestClient client) : base(BuildCurl(response, client), response.ErrorException)
        {
            StatusCode = response.StatusCode;
            ResponseContent = response.Content;
        }

        public static string BuildCurl(IRestResponse response, IRestClient client)
        {
            var sb = new StringBuilder();

            sb.Append($"**API REQUEST**:\ncurl -X {response.Request.Method}");

            var p = new List<Parameter>();
            p.AddRange(client.DefaultParameters);
            p.AddRange(response.Request.Parameters);

            foreach (var h in p)
            {
                switch (h.Type)
                {
                    case ParameterType.HttpHeader:
                        sb.Append($" --header \"{h.Name}: {h.Value}\"");
                        break;
                    case ParameterType.RequestBody:
                        sb.Append($" --data-raw '{response.Request.JsonSerializer.Serialize(h.Value)}'".Replace(Environment.NewLine, ""));
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
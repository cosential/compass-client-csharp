using System;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;

//ToDo: Create a git hub account for Cosential to host this project as an open source project (along with unit tests)

namespace Cosential.Integrations.CompassApiClient
{
    public class Client
    {
        private readonly RestClient _client;

        public static readonly Uri DefaultUri = new Uri("https://compass.cosential.com/api");

        public Client(int firmId, Guid apiKey, string username, string password, Uri host= null)
        {
            if (host == null) host = DefaultUri;

            _client = new RestClient(host)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };

            _client.AddDefaultHeader("x-compass-api-key", apiKey.ToString());
            _client.AddDefaultHeader("x-compass-firm-id", firmId.ToString());
            _client.AddDefaultHeader("Accept", "application/json");
        }

        public IRestResponse Execute(RestRequest request)
        {
            var res = _client.Execute(request);
            ValidateResponse(res);
            return res;
        }

        public IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            var res = _client.Execute<T>(request);
            ValidateResponse(res);
            return res;
        }

        private static void ValidateResponse(IRestResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK) throw new ResponseStatusCodeException(response);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace Cosential.Integrations.Compass.Client
{
    public class CompassClient
    {
        private readonly RestClient _client;

        public static readonly Uri DefaultUri = new Uri("https://compass.cosential.com/api");

        public CompassClient(int firmId, Guid apiKey, string username, string password, Uri host= null)
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

        public bool IsAuth()
        {
            var user = GetAuthenticatedUser();
            return (user != null);
        }
        public AuthenticatedUser GetAuthenticatedUser()
        {
            try
            {
                var request = new RestRequest("user", Method.GET) {RequestFormat = DataFormat.Json};
                var result = Execute<List<AuthenticatedUser>>(request);
                return result.Data.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
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

        public async Task<IRestResponse<T>> ExecuteAsyc<T>(RestRequest request, CancellationToken cancel)
        {
            var res = await _client.ExecuteTaskAsync<T>(request, cancel);
            ValidateResponse(res);
            return res;
        }

        private static void ValidateResponse(IRestResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK) throw new ResponseStatusCodeException(response);
        }
    }
}

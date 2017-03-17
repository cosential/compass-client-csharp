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
        public readonly JsonSerializer _json;

        public CompassClient(int firmId, Guid apiKey, string username, string password, Uri host= null)
        {
            if (host == null) host = DefaultUri;

            _json = new JsonSerializer();

            _client = new RestClient(host)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
                
            };

            _client.ClearHandlers();

            _client.AddDefaultHeader("x-compass-api-key", apiKey.ToString());
            _client.AddDefaultHeader("x-compass-firm-id", firmId.ToString());
            _client.AddDefaultHeader("Accept", "application/json");
            
            _client.AddHandler("application/json", _json);
            _client.AddHandler("text/json", _json);
            _client.AddHandler("text/x-json", _json);
            _client.AddHandler("text/javascript", _json);
            _client.AddHandler("*+json", _json);
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
                var request = new RestRequest("user", Method.GET);
                var result = Execute<List<AuthenticatedUser>>(request);
                return result.Data.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<T> GetSubItems<T>(PrimaryEntityType entityType, int entityId, string subitem)
        {
            var request = new RestRequest("{entityType}/{id}/{subitem}", Method.GET);
            request.AddUrlSegment("entityType", entityType.ToPlural());
            request.AddUrlSegment("id", entityId.ToString());
            request.AddUrlSegment("subitem", subitem);

            var results = Execute<List<T>>(request);

            return results.Data;
        }

        public IRestResponse Execute(RestRequest request)
        {
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = _json;

            var res = _client.Execute(request);
            ValidateResponse(res);
            return res;
        }

        public IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = _json;

            var res = _client.Execute<T>(request);
            ValidateResponse(res);
            return res;
        }

        public async Task<IRestResponse<T>> ExecuteAsync<T>(RestRequest request, CancellationToken cancel)
        {
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = _json;

            var res = await _client.ExecuteTaskAsync<T>(request, cancel);
            ValidateResponse(res);
            return res;
        }

        private static void ValidateResponse(IRestResponse response)
        {
            //if (response.ErrorException != null) throw new HttpResponseException($"Exception in http response from [{response.ResponseUri}]", response.ErrorException);
            if (response.StatusCode != HttpStatusCode.OK) throw new ResponseStatusCodeException(response);
            
        }
    }
}

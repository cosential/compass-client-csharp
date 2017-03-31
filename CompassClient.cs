using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Contexts;
using Cosential.Integrations.Compass.Client.Exceptions;
using Cosential.Integrations.Compass.Client.Models;
using Cosential.Integrations.Compass.Contexts;
using RestSharp;
using RestSharp.Authenticators;

namespace Cosential.Integrations.Compass.Client
{
    public class CompassClient : IDisposable
    {
        private readonly RestClient _client;

        public static readonly Uri DefaultUri = new Uri("https://compass.cosential.com/api");
        public readonly JsonSerializer Json;

        public PersonnelContext PersonnelContext { get; private set; }
        public CompanyContext CompanyContext { get; private set; }
        public OfficeContext OfficeContext { get; private set; }
        public DivisionContext DivisionContext { get; private set; }
        public StudioContext StudioContext { get; set; }
        public TerritoryContext TerritoryContext { get; set; }
        public PracticeAreaContext PracticeAreaContext { get; set; }
        public OfficeDivisionContext OfficeDivisionContext { get; set; }
        public OpportunityContext OpportunityContext { get; set; }

        public CompassClient(int firmId, Guid apiKey, string username, string password, Uri host= null)
        {
            if (host == null) host = DefaultUri;

            Json = new JsonSerializer();

            _client = new RestClient(host)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
                
            };

            _client.ClearHandlers();

            _client.AddDefaultHeader("x-compass-api-key", apiKey.ToString());
            _client.AddDefaultHeader("x-compass-firm-id", firmId.ToString());
            _client.AddDefaultHeader("Accept", "application/json");
            _client.AddDefaultHeader("x-compass-show-error", "true");
            
            _client.AddHandler("application/json", Json);
            _client.AddHandler("text/json", Json);
            _client.AddHandler("text/x-json", Json);
            _client.AddHandler("text/javascript", Json);
            _client.AddHandler("*+json", Json);

            PersonnelContext = new PersonnelContext(this);
            CompanyContext = new CompanyContext(this);
            OfficeContext = new OfficeContext(this);
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
                var request = NewRequest("user");
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
            var request = NewRequest("{entityType}/{id}/{subitem}");
            request.AddUrlSegment("entityType", entityType.ToPlural());
            request.AddUrlSegment("id", entityId.ToString());
            request.AddUrlSegment("subitem", subitem);

            var results = Execute<List<T>>(request);

            return results.Data;
        }

        public RestRequest NewRequest(string resource, Method method = Method.GET)
        {
            var request = new RestRequest(resource, method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = Json
            };

            return request;

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

        public async Task<IRestResponse<T>> ExecuteAsync<T>(RestRequest request, CancellationToken cancel)
        {
            var res = await _client.ExecuteTaskAsync<T>(request, cancel);
            ValidateResponse(res);
            return res;
        }

        public async Task<IRestResponse> ExecuteAsync(RestRequest request, CancellationToken cancel)
        {
            var res = await _client.ExecuteTaskAsync(request, cancel);
            ValidateResponse(res);
            return res;
        }

        private static void ValidateResponse(IRestResponse response)
        {
            if (response.ErrorException != null) throw new HttpResponseException($"Exception in http response from [{response.ResponseUri}]", response.ErrorException);

            if (response.StatusCode == HttpStatusCode.OK) return;
            if (response.StatusCode == HttpStatusCode.NotFound) return;

            throw new ResponseStatusCodeException(response);
        }

        public void Dispose()
        {
            
        }
    }
}

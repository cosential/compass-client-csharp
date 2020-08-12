using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Contexts;
using Cosential.Integrations.Compass.Client.Exceptions;
using Cosential.Integrations.Compass.Client.Models;
using Cosential.Integrations.Compass.Client.Models.Interfaces;
using Cosential.Integrations.Compass.Contexts;
using log4net;
using RestSharp;
using RestSharp.Authenticators;

namespace Cosential.Integrations.Compass.Client
{
    public class CompassClient : IDisposable
    {
        private bool _isDisposed;
        private readonly RestClient _client;
        private readonly ILog _log;

        public static readonly Uri DefaultUri = new Uri("https://compass.cosential.com/api");
        public JsonSerializer Json { get; }

        private PersonnelContext _personnelContext;
        public PersonnelContext PersonnelContext => _personnelContext ?? (_personnelContext = new PersonnelContext(this));

        private CompanyContext _companyContext;
        public CompanyContext CompanyContext => _companyContext ?? (_companyContext = new CompanyContext(this));

        private CompanyAddressContext _companyAddressContext;
        public CompanyAddressContext CompanyAddressContext => _companyAddressContext ?? (_companyAddressContext = new CompanyAddressContext(this));

        private OfficeContext _officeContext;
        public OfficeContext OfficeContext => _officeContext ?? (_officeContext = new OfficeContext(this));

        private DivisionContext _divisionContext;
        public DivisionContext DivisionContext => _divisionContext ?? (_divisionContext = new DivisionContext(this));

        private StudioContext _studioContext;
        public StudioContext StudioContext => _studioContext ?? (_studioContext = new StudioContext(this));

        private TerritoryContext _territoryContext;
        public TerritoryContext TerritoryContext => _territoryContext ?? (_territoryContext = new TerritoryContext(this));

        private PracticeAreaContext _practiceAreaContext;
        public PracticeAreaContext PracticeAreaContext => _practiceAreaContext ?? (_practiceAreaContext = new PracticeAreaContext(this));

        private OfficeDivisionContext _officeDivisionContext;
        public OfficeDivisionContext OfficeDivisionContext => _officeDivisionContext ?? (_officeDivisionContext  = new OfficeDivisionContext(this));

        private OpportunityContext _opportunityContext;
        public OpportunityContext OpportunityContext => _opportunityContext ?? (_opportunityContext = new OpportunityContext(this));

        private StageContext _stageContext;
        public StageContext StageContext => _stageContext ?? (_stageContext = new StageContext(this));

        private ContactContext _contactContext;
        public ContactContext ContactContext => _contactContext ?? (_contactContext = new ContactContext(this));

        private ContactAddressContext _contactAddressContext;
        public ContactAddressContext ContactAddressContext => _contactAddressContext ?? (_contactAddressContext = new ContactAddressContext(this));

        private ProjectContext _projectContext;
        public ProjectContext ProjectContext => _projectContext ?? (_projectContext = new ProjectContext(this));

        private StaffTeamContext _staffTeamContext;
        public StaffTeamContext StaffTeamContext => _staffTeamContext ?? (_staffTeamContext = new StaffTeamContext(this));

        private OpportunityCompetitorContext _opportunityCompetitorContext;
        public OpportunityCompetitorContext OpportunityCompetitorContext => _opportunityCompetitorContext ?? (_opportunityCompetitorContext = new OpportunityCompetitorContext(this));

        private OpportunityRevenueProjectionContext _opportunityRevenueProjectionContext;
        public OpportunityRevenueProjectionContext OpportunityRevenueProjectionContext => _opportunityRevenueProjectionContext ?? (_opportunityRevenueProjectionContext = new OpportunityRevenueProjectionContext(this));


        public ValueListContext<T> GetValueListContext<T>() where T : IValueList
        {
            return new ValueListContext<T>(this);
        }

        public CompassClient(int firmId, Guid apiKey, string username, string password, Uri host = null)
        {
            if (host == null) host = DefaultUri;

            _log = LogManager.GetLogger(typeof(CompassClient));

            Json = new JsonSerializer();

            _client = new RestClient(host)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)

            };

            _client.ClearHandlers();

            _client.AddDefaultHeader("x-compass-api-key", apiKey.ToString());
            _client.AddDefaultHeader("x-compass-firm-id", firmId.ToString(CultureInfo.InvariantCulture));
            _client.AddDefaultHeader("Accept", "application/json");
            _client.AddDefaultHeader("x-compass-show-error", "true");

            _client.AddHandler("application/json", () => Json);
            _client.AddHandler("text/json", () => Json);
            _client.AddHandler("text/x-json", () => Json);
            _client.AddHandler("text/javascript", () => Json);
            _client.AddHandler("*+json", () => Json);
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
            request.AddUrlSegment("id", entityId.ToString(CultureInfo.InvariantCulture));
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
            //var ts = DateTime.Now;
            var res = _client.Execute(request);
            //_log.Debug($"Call took [{DateTime.Now.Subtract(ts)}] to [{res.ResponseUri}]");
            ValidateResponse(res);
            return res;
        }

        public IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            var attempts = 1;
            var res = _client.Execute<T>(request);
            while (!res.IsSuccessful && attempts < 5)
            {
                Thread.Sleep(100);
                res = _client.Execute<T>(request);
                attempts++;
            }
            ValidateResponse(res);
            return res;
        }

        public async Task<IRestResponse<T>> ExecuteAsync<T>(RestRequest request, CancellationToken cancel)
        {
            var attempts = 1;
            var res = await _client.ExecuteAsync<T>(request, cancel).ConfigureAwait(false);

            while (!res.IsSuccessful && attempts < 5)
            {
                Thread.Sleep(100);
                res = await _client.ExecuteAsync<T>(request, cancel).ConfigureAwait(false);
                attempts++;
            }

            //_log.Debug($"Call took [{DateTime.Now.Subtract(ts)}] to [{res.ResponseUri}]");
            ValidateResponse(res);
            return res;
        }

        public async Task<IRestResponse> ExecuteAsync(RestRequest request, CancellationToken cancel)
        {
            //var ts = DateTime.Now;
            var attempts = 1;
            var res = await _client.ExecuteAsync(request, cancel).ConfigureAwait(false);
            
            while (!res.IsSuccessful && attempts < 5)
            {
                Thread.Sleep(100);
                res = await _client.ExecuteAsync(request, cancel).ConfigureAwait(false);
                attempts++;
            }

            //_log.Debug($"Call took [{DateTime.Now.Subtract(ts)}] to [{res.ResponseUri}]");
            ValidateResponse(res);
            return res;
        }

        private void ValidateResponse(IRestResponse response)
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug(ResponseStatusCodeException.BuildCurl(response, _client));
            }

            if (response.ErrorException != null || response.StatusCode == HttpStatusCode.InternalServerError)
                throw new ResponseStatusCodeException(response, _client);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // free managed resources
            }

            _isDisposed = true;
        }
    }
}

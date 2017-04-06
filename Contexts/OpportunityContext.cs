using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client;
using Cosential.Integrations.Compass.Client.Contexts;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public class OpportunityContext : ICompassContext<Opportunity>
    {

        private readonly CompassClient _client;

        public OpportunityContext(CompassClient client )
        {
            _client = client;
        }

        #region CRUD

        public Opportunity Get(int opportunityId)
        {
            var task = GetAsync(opportunityId, CancellationToken.None);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<Opportunity> GetAsync(int opportunityId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportunities/{id}");
            request.AddUrlSegment("id", opportunityId.ToString());

            var results = await _client.ExecuteAsync<Opportunity>(request, cancelToken);
            return results.Data;
        }

        public async Task<Opportunity> UpdateAsync(Opportunity entity, CancellationToken cancel)
        {
            var request = _client.NewRequest("opportunities/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.OpportunityId.ToString());
            request.AddBody(entity);

            var results = await _client.ExecuteAsync<Opportunity>(request, cancel);
            return results.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancel)
        {
            var request = _client.NewRequest("opportunities/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());

            await _client.ExecuteAsync<Opportunity>(request, cancel);
        }

        public async Task<Opportunity> CreateAsync(Opportunity entity, CancellationToken cancel)
        {
            var result = await CreateAsync(new[] { entity }, cancel);
            return result.FirstOrDefault();
        }

        public async Task<IList<Opportunity>> CreateAsync(IEnumerable<Opportunity> entities, CancellationToken cancel)
        {
            var request = _client.NewRequest("opportunities", Method.POST);
            request.AddBody(entities);

            var results = await _client.ExecuteAsync<List<Opportunity>>(request, cancel);
            return results.Data;
        }

        public async Task<UpsertResult<Opportunity>> UpsertAsync(Opportunity entity, CancellationToken cancelToken)
        {
            var result = new UpsertResult<Opportunity>();

            if (entity.OpportunityId.HasValue && entity.OpportunityId.Value > 0)
            {
                result.Action = UpsertAction.Updated;
                result.Data = await UpdateAsync(entity, cancelToken);
            }
            else
            {
                result.Action = UpsertAction.Created;
                result.Data = await CreateAsync(entity, cancelToken);
            }

            return result;
        }

        public List<Opportunity> List(int from, int size, bool full = true)
        {
            var request = _client.NewRequest("opportunities", Method.GET);
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", size.ToString());
            request.AddQueryParameter("full", full.ToString());

            var results = _client.Execute<List<Opportunity>>(request);
            if (results.Data == null)
            {
                Debug.Write(results.Content);
            }
            return results.Data;
        }

        #endregion

        #region Changes

        public IList<ChangeEvent> GetChanges(byte[] version = null, bool includeDeleted = false)
        {
            var task = GetChangesAsync(version, includeDeleted, CancellationToken.None);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] version, bool includeDeleted, CancellationToken cancel)
        {
            var request = _client.NewRequest("opportunities/changes");
            if (version != null) request.AddQueryParameter("version", Convert.ToBase64String(version));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel);
            return results.Data;
        }

        #endregion


        #region Subitems
        public List<Office> GetOffices(int opportunityId)
        {
            return _client.GetSubItems<Office>(PrimaryEntityType.Opportunity, opportunityId, "offices");
        }

        public List<Studio> GetStudios(int opportunityId)
        {
            return _client.GetSubItems<Studio>(PrimaryEntityType.Opportunity, opportunityId, "studios");
        }

        public List<OfficeDivision> GetOfficeDivisions(int opportunityId)
        {
            return _client.GetSubItems<OfficeDivision>(PrimaryEntityType.Opportunity, opportunityId, "officedivisions");
        }

        public List<PracticeArea> GetPracticeAreas(int opportunityId)
        {
            return _client.GetSubItems<PracticeArea>(PrimaryEntityType.Opportunity, opportunityId, "practiceareas");
        }

        public List<Territory> GetTerritories(int opportunityId)
        {
            return _client.GetSubItems<Territory>(PrimaryEntityType.Opportunity, opportunityId, "territories");
        }

        #endregion

        public async Task<Dictionary<string, object>> GetMetadataAync(MetadataScope scope, int id, CancellationToken cancellationToken)
        {
            var request = _client.NewRequest("opportunities/{id}/metadata/{scope}");
            request.AddUrlSegment("id", id.ToString());
            request.AddUrlSegment("scope", scope.ToString());

            var result = await _client.ExecuteAsync<Dictionary<string, object>>(request, cancellationToken);
            return result.Data ?? new Dictionary<string, object>();
        }
    }
}

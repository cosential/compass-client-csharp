using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public class OpportunityCompetitorContext : ICompassContext<OpportunityCompetitor>
    {
        private readonly CompassClient _client;
        public OpportunityCompetitorContext(CompassClient client)
        {
            _client = client;
        }

        #region CRUD

        public OpportunityCompetitor Get(int opportunityId, int competitorId)
        {
            var task = GetAsync(opportunityId, CancellationToken.None, competitorId);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<OpportunityCompetitor> GetAsync(int competitorId, CancellationToken cancelToken, int? parentId = null)
        {
            if (!parentId.HasValue) throw new ArgumentException("Parent id value is required to get opportunity competitor.");

            var request = _client.NewRequest("opportunities/{id}/competition/{competitorId}");
            request.AddUrlSegment("id", parentId.Value);
            request.AddUrlSegment("competitorId", competitorId);

            var results = await _client.ExecuteAsync<OpportunityCompetitor>(request, cancelToken).ConfigureAwait(false);
            return results.Data;
        }

        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<OpportunityCompetitor> UpdateAsync(OpportunityCompetitor entity, CancellationToken cancel)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id, CancellationToken cancel, int? parentId = null)
        {
            throw new NotImplementedException();
        }


        public async Task<OpportunityCompetitor> CreateAsync(OpportunityCompetitor entity, CancellationToken cancel,
            int? parentId = null)
        {
            throw new NotImplementedException(); 
        }

        #pragma warning disable CA1068
        [Obsolete("Use public async Task<IList<OpportunityCompetitor>> CreateAsync(IEnumerable<OpportunityCompetitor> entities, int parentId, CancellationToken cancel)", false)]
        public async Task<IList<OpportunityCompetitor>> CreateAsync(IEnumerable<OpportunityCompetitor> entities, CancellationToken cancel, int parentId)
        {
            throw new NotImplementedException();
        }
        #pragma warning restore CA1068

        public async Task<IList<OpportunityCompetitor>> CreateAsync(IEnumerable<OpportunityCompetitor> entities, int parentId, CancellationToken cancel)
        {
            throw new NotImplementedException();
        }

        public async Task<UpsertResult<OpportunityCompetitor>> UpsertAsync(OpportunityCompetitor entity,
            CancellationToken cancelToken, int? parentId = null)
        {
            throw new NotImplementedException();
        }
        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        public List<OpportunityCompetitor> List(int opportunityId, int from, int size)
        {
            var request = _client.NewRequest("opportunities/{opportunityId}/competition");
            request.AddUrlSegment("contactId", opportunityId);
            request.AddQueryParameter("from", from.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("size", size.ToString(CultureInfo.InvariantCulture));

            var results = _client.Execute<List<OpportunityCompetitor>>(request);
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
            var request = _client.NewRequest("opportunities/competition/changes");
            if (version != null) request.AddQueryParameter("version", Convert.ToBase64String(version));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel).ConfigureAwait(false);
            return results.Data;
        }


        public async Task<TM> GetMetadataAync<TM>(MetadataScope scope, int id, CancellationToken cancellationToken,
            int? parentId = null)
        {
            var request = _client.NewRequest("opportunities/{id}/metadata/{scope}");
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("scope", scope.ToString());

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken).ConfigureAwait(false);
            return result.Data;
        }

        public async Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data,
            CancellationToken cancellationToken, int? parentId = null)
        {
            var request = _client.NewRequest("opportunities/{id}/metadata/{scope}", Method.PUT);
            request.AddUrlSegment("id", entityId.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("scope", scope.ToString());
            request.AddJsonBody(data);

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken).ConfigureAwait(false);
            return result.Data;
        }

        #endregion
    }
}

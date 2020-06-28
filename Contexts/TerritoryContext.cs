using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public class TerritoryContext : ICompassContext<Territory>
    {

        private readonly CompassClient _client;

        public TerritoryContext(CompassClient client)
        {
            _client = client;
        }

        public async Task<Territory> GetAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest($"firmorgs/territories/{id}");
            var results = await _client.ExecuteAsync<Territory>(request, cancelToken).ConfigureAwait(false);

            return results.Data;

        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            var request = _client.NewRequest("firmorgs/territories/changes");
            request.AddQueryParameter("Version", Convert.ToBase64String(rowVersion));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());

            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token).ConfigureAwait(false);

            return results.Data;
        }

        public async Task<UpsertResult<Territory>> UpsertAsync(Territory entity, CancellationToken cancelToken,
            int? parentId = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var result = new UpsertResult<Territory>();

            if (entity.TerritoryID.HasValue && entity.TerritoryID.Value > 0)
            {
                result.Action = UpsertAction.Updated;
                result.Data = await UpdateAsync(entity, cancelToken).ConfigureAwait(false);
            }
            else
            {
                result.Action = UpsertAction.Created;
                result.Data = await CreateAsync(entity, cancelToken).ConfigureAwait(false);
            }

            return result;
        }

        public async Task<Territory> CreateAsync(Territory entity, CancellationToken cancelToken, int? parentId = null)
        {
            var divisions = await CreateAsync(new[] { entity }, cancelToken).ConfigureAwait(false);
            return divisions.FirstOrDefault();
        }

        public async Task<List<Territory>> CreateAsync(IEnumerable<Territory> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/territories", Method.POST);
            request.AddJsonBody(entities);

            var response = await _client.ExecuteAsync<List<Territory>>(request, cancelToken).ConfigureAwait(false);

            return response.Data;
        }

        public async Task<Territory> UpdateAsync(Territory entity, CancellationToken cancelToken)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var request = _client.NewRequest("firmorgs/territories/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.TerritoryID.ToString());
            request.AddJsonBody(entity);

            var response = await _client.ExecuteAsync<Territory>(request, cancelToken).ConfigureAwait(false);

            return response.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("firmorgs/territories/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));

            await _client.ExecuteAsync(request, cancelToken).ConfigureAwait(false);
        }

        public Task<TM> GetMetadataAync<TM>(MetadataScope scope, int entityId, CancellationToken cancellationToken,
            int? parentId = null)
        {
            return Task.FromResult(default(TM));
        }

        public Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data,
            CancellationToken cancellationToken, int? parentId = null)
        {
            return Task.FromResult(default(TM));
        }
    }
}

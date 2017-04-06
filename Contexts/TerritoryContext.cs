using System;
using System.Collections.Generic;
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

        public async Task<Territory> GetAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest($"firmorgs/territories/{id}");
            var results = await _client.ExecuteAsync<Territory>(request, cancelToken);

            return results.Data;

        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            var request = _client.NewRequest("firmorgs/territories/changes");
            request.AddQueryParameter("Version", Convert.ToBase64String(rowVersion));
            request.AddQueryParameter("includeDeleted", true.ToString());

            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token);

            return results.Data;
        }

        public async Task<UpsertResult<Territory>> UpsertAsync(Territory entity, CancellationToken cancelToken)
        {
            var result = new UpsertResult<Territory>();

            if (entity.TerritoryID.HasValue && entity.TerritoryID.Value > 0)
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

        public async Task<Territory> CreateAsync(Territory entity, CancellationToken cancelToken)
        {
            var divisions = await CreateAsync(new[] { entity }, cancelToken);
            return divisions.FirstOrDefault();
        }

        public async Task<List<Territory>> CreateAsync(IEnumerable<Territory> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/territories", Method.POST);
            request.AddBody(entities);

            var response = await _client.ExecuteAsync<List<Territory>>(request, cancelToken);

            return response.Data;
        }

        public async Task<Territory> UpdateAsync(Territory entity, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/territories/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.TerritoryID.ToString());
            request.AddBody(entity);

            var response = await _client.ExecuteAsync<Territory>(request, cancelToken);

            return response.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/territories/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());

            await _client.ExecuteAsync(request, cancelToken);
        }

        public Task<Dictionary<string, object>> GetMetadataAync(MetadataScope scope, int id, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Dictionary<string, object>());
        }
    }
}

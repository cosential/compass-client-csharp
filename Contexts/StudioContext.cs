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
    public class StudioContext : ICompassContext<Studio>
    {

        private readonly CompassClient _client;

        public StudioContext(CompassClient client)
        {
            _client = client;
        }

        public async Task<Studio> GetAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest($"firmorgs/studios/{id}");
            var results = await _client.ExecuteAsync<Studio>(request, cancelToken);

            return results.Data;

        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            var request = _client.NewRequest("firmorgs/studios/changes");
            request.AddQueryParameter("Version", Convert.ToBase64String(rowVersion));
            request.AddQueryParameter("includeDeleted", true.ToString());

            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token);

            return results.Data;
        }

        public async Task<UpsertResult<Studio>> UpsertAsync(Studio entity, CancellationToken cancelToken)
        {
            var result = new UpsertResult<Studio>();

            if (entity.StudioID.HasValue && entity.StudioID.Value > 0)
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

        public async Task<Studio> CreateAsync(Studio entity, CancellationToken cancelToken)
        {
            var divisions = await CreateAsync(new[] { entity }, cancelToken);
            return divisions.FirstOrDefault();
        }

        public async Task<List<Studio>> CreateAsync(IEnumerable<Studio> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/studios", Method.POST);
            request.AddBody(entities);

            var response = await _client.ExecuteAsync<List<Studio>>(request, cancelToken);

            return response.Data;
        }

        public async Task<Studio> UpdateAsync(Studio entity, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/studios/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.StudioID.ToString());
            request.AddBody(entity);

            var response = await _client.ExecuteAsync<Studio>(request, cancelToken);

            return response.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/studios/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());

            await _client.ExecuteAsync(request, cancelToken);
        }

        public Task<Dictionary<string, object>> GetMetadataAync(MetadataScope scope, int id, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Dictionary<string, object>());
        }

    }
}

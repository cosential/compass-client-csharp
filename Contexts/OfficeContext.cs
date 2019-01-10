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
    public class OfficeContext : ICompassContext<Office>
    {
        private readonly CompassClient _client;

        public OfficeContext(CompassClient client)
        {
            _client = client;
        }

        public async Task<Office> GetAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest($"firmorgs/offices/{id}");
            var results = await _client.ExecuteAsync<Office>(request, cancelToken);

            return results.Data;
        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            var request = _client.NewRequest("firmorgs/offices/changes");
            request.AddQueryParameter("version", Convert.ToBase64String(rowVersion));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token);
            return results.Data;
        }

        public async Task<UpsertResult<Office>> UpsertAsync(Office entity, CancellationToken cancelToken,
            int? parentId = null)
        {
            var result = new UpsertResult<Office>();

            if (entity.OfficeID.HasValue && entity.OfficeID.Value > 0)
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

        public async Task<Office> CreateAsync(Office entity, CancellationToken cancelToken, int? parentId = null)
        {
            var offices = await CreateAsync(new [] {entity}, cancelToken);
            return offices.FirstOrDefault();
        }

        private async Task<List<Office>> CreateAsync(IEnumerable<Office> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/offices", Method.POST);
            request.AddBody(entities);

            var response = await _client.ExecuteAsync<List<Office>>(request, cancelToken);
            return response.Data;
        }

        public async Task<Office> UpdateAsync(Office entity, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/offices/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.OfficeID.ToString());
            request.AddBody(entity);

            var response = await _client.ExecuteAsync<Office>(request, cancelToken);
            return response.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("firmorgs/offices/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());

            await _client.ExecuteAsync(request, cancelToken);
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

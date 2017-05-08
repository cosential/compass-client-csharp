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
    public class OfficeDivisionContext : ICompassContext<OfficeDivision>
    {

        private readonly CompassClient _client;

        public OfficeDivisionContext(CompassClient client)
        {
            _client = client;
        }

        public async Task<OfficeDivision> GetAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest($"firmorgs/officedivisions/{id}");
            var results = await _client.ExecuteAsync<OfficeDivision>(request, cancelToken);

            return results.Data;

        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            var request = _client.NewRequest("firmorgs/officedivisions/changes");
            request.AddQueryParameter("Version", Convert.ToBase64String(rowVersion));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());

            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token);

            return results.Data;
        }

        public async Task<UpsertResult<OfficeDivision>> UpsertAsync(OfficeDivision entity, CancellationToken cancelToken)
        {
            var result = new UpsertResult<OfficeDivision>();

            if (entity.OfficeDivisionID.HasValue && entity.OfficeDivisionID.Value > 0)
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

        public async Task<OfficeDivision> CreateAsync(OfficeDivision entity, CancellationToken cancelToken)
        {
            var divisions = await CreateAsync(new[] { entity }, cancelToken);
            return divisions.FirstOrDefault();
        }

        public async Task<List<OfficeDivision>> CreateAsync(IEnumerable<OfficeDivision> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/officedivisions", Method.POST);
            request.AddBody(entities);

            var response = await _client.ExecuteAsync<List<OfficeDivision>>(request, cancelToken);

            return response.Data;
        }

        public async Task<OfficeDivision> UpdateAsync(OfficeDivision entity, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/officedivisions/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.OfficeDivisionID.ToString());
            request.AddBody(entity);

            var response = await _client.ExecuteAsync<OfficeDivision>(request, cancelToken);

            return response.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/officedivisions/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());

            await _client.ExecuteAsync(request, cancelToken);
        }

        public Task<Dictionary<string, object>> GetMetadataAync(MetadataScope scope, int id, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Dictionary<string, object>());
        }

        public Task<Dictionary<string, object>> PutMetadataAsync(MetadataScope scope, int entityId, Dictionary<string, object> data, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Dictionary<string, object>());
        }
    }
}

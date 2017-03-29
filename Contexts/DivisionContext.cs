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
    public class DivisionContext : ICompassContext<Division>
    {

        private readonly CompassClient _client;

        public DivisionContext(CompassClient client)
        {
            _client = client;
        }

        public async Task<Division> GetAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest($"firmorgs/divisions/{id}");
            var results = await _client.ExecuteAsync<Division>(request, cancelToken);

            return results.Data;

        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            var request = _client.NewRequest("firmorgs/divisions/changes");
                request.AddQueryParameter("Version", Convert.ToBase64String(rowVersion));
                request.AddQueryParameter("includeDeleted", true.ToString());

            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token);

            return results.Data;
        }

        public async Task<UpsertResult<Division>> UpsertAsync( Division entity, CancellationToken cancelToken)
        {
            var result = new UpsertResult<Division>();

            if (entity.DivisionID.HasValue && entity.DivisionID.Value > 0)
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

        public async Task<Division> CreateAsync( Division entity, CancellationToken cancelToken)
        {
            var divisions = await CreateAsync(new [] {entity}, cancelToken);
            return divisions.FirstOrDefault();
        }

        public async Task<List<Division>> CreateAsync( IEnumerable<Division> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/divisions", Method.POST);
            request.AddBody(entities);

            var response = await _client.ExecuteAsync<List<Division>>(request, cancelToken);

            return response.Data;
        }

        public async Task<Division> UpdateAsync( Division entity, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/divisions/{id}", Method.PUT);
                request.AddUrlSegment("id", entity.DivisionID.ToString());
                request.AddBody(entity);

            var response = await _client.ExecuteAsync<Division>(request, cancelToken);

            return response.Data;
        }

        public async Task DeleteAsync( int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/offices/{id}", Method.DELETE);
                request.AddUrlSegment("id", id.ToString());

            await _client.ExecuteAsync(request, cancelToken);
        }

    }
}

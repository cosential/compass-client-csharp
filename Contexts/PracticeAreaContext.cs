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
    public class PracticeAreaContext : ICompassContext<PracticeArea>
    {

        private readonly CompassClient _client;

        public PracticeAreaContext(CompassClient client)
        {
            _client = client;
        }

        public async Task<PracticeArea> GetAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest($"firmorgs/practiceareas/{id}");
            var results = await _client.ExecuteAsync<PracticeArea>(request, cancelToken);

            return results.Data;

        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            var request = _client.NewRequest("firmorgs/practiceareas/changes");
            request.AddQueryParameter("Version", Convert.ToBase64String(rowVersion));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());

            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token);

            return results.Data;
        }

        public async Task<UpsertResult<PracticeArea>> UpsertAsync(PracticeArea entity, CancellationToken cancelToken)
        {
            var result = new UpsertResult<PracticeArea>();

            if (entity.PracticeAreaID.HasValue && entity.PracticeAreaID.Value > 0)
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

        public async Task<PracticeArea> CreateAsync(PracticeArea entity, CancellationToken cancelToken)
        {
            var divisions = await CreateAsync(new[] { entity }, cancelToken);
            return divisions.FirstOrDefault();
        }

        public async Task<List<PracticeArea>> CreateAsync(IEnumerable<PracticeArea> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/practiceareas", Method.POST);
            request.AddBody(entities);

            var response = await _client.ExecuteAsync<List<PracticeArea>>(request, cancelToken);

            return response.Data;
        }

        public async Task<PracticeArea> UpdateAsync(PracticeArea entity, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/practiceareas/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.PracticeAreaID.ToString());
            request.AddBody(entity);

            var response = await _client.ExecuteAsync<PracticeArea>(request, cancelToken);

            return response.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/practiceareas/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());

            await _client.ExecuteAsync(request, cancelToken);
        }

        public Task<TM> GetMetadataAync<TM>(MetadataScope scope, int entityId, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(TM));
        }

        public Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(TM));
        }
    }
}

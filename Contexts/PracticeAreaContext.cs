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
    public class PracticeAreaContext : ICompassContext<PracticeArea>
    {

        private readonly CompassClient _client;

        public PracticeAreaContext(CompassClient client)
        {
            _client = client;
        }

        public async Task<PracticeArea> GetAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest($"firmorgs/practiceareas/{id}");
            var results = await _client.ExecuteAsync<PracticeArea>(request, cancelToken).ConfigureAwait(false);

            return results.Data;

        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            var request = _client.NewRequest("firmorgs/practiceareas/changes");
            request.AddQueryParameter("Version", Convert.ToBase64String(rowVersion));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());

            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token).ConfigureAwait(false);

            return results.Data;
        }

        public async Task<UpsertResult<PracticeArea>> UpsertAsync(PracticeArea entity, CancellationToken cancelToken,
            int? parentId = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var result = new UpsertResult<PracticeArea>();

            if (entity.PracticeAreaID.HasValue && entity.PracticeAreaID.Value > 0)
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

        public async Task<PracticeArea> CreateAsync(PracticeArea entity, CancellationToken cancelToken,
            int? parentId = null)
        {
            var divisions = await CreateAsync(new[] { entity }, cancelToken).ConfigureAwait(false);
            return divisions.FirstOrDefault();
        }

        public async Task<List<PracticeArea>> CreateAsync(IEnumerable<PracticeArea> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/practiceareas", Method.POST);
            request.AddJsonBody(entities);

            var response = await _client.ExecuteAsync<List<PracticeArea>>(request, cancelToken).ConfigureAwait(false);

            return response.Data;
        }

        public async Task<PracticeArea> UpdateAsync(PracticeArea entity, CancellationToken cancelToken)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var request = _client.NewRequest("firmorgs/practiceareas/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.PracticeAreaID.ToString());
            request.AddJsonBody(entity);

            var response = await _client.ExecuteAsync<PracticeArea>(request, cancelToken).ConfigureAwait(false);

            return response.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("firmorgs/practiceareas/{id}", Method.DELETE);
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

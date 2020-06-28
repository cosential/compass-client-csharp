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
            var results = await _client.ExecuteAsync<Office>(request, cancelToken).ConfigureAwait(false);

            return results.Data;
        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            var request = _client.NewRequest("firmorgs/offices/changes");
            request.AddQueryParameter("version", Convert.ToBase64String(rowVersion));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token).ConfigureAwait(false);
            return results.Data;
        }

        public async Task<UpsertResult<Office>> UpsertAsync(Office entity, CancellationToken cancelToken,
            int? parentId = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var result = new UpsertResult<Office>();

            if (entity.OfficeID.HasValue && entity.OfficeID.Value > 0)
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

        public async Task<Office> CreateAsync(Office entity, CancellationToken cancelToken, int? parentId = null)
        {
            var offices = await CreateAsync(new [] {entity}, cancelToken).ConfigureAwait(false);
            return offices.FirstOrDefault();
        }

        private async Task<List<Office>> CreateAsync(IEnumerable<Office> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("firmorgs/offices", Method.POST);
            request.AddJsonBody(entities);

            var response = await _client.ExecuteAsync<List<Office>>(request, cancelToken).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<Office> UpdateAsync(Office entity, CancellationToken cancelToken)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));
            if(!entity.OfficeID.HasValue)throw new ArgumentException("Entity does not have an office id.");

            var request = _client.NewRequest("firmorgs/offices/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.OfficeID.Value.ToString(CultureInfo.InvariantCulture));
            request.AddJsonBody(entity);

            var response = await _client.ExecuteAsync<Office>(request, cancelToken).ConfigureAwait(false);
            return response.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("firmorgs/offices/{id}", Method.DELETE);
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

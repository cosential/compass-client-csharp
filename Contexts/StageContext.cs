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
    public class StageContext : ICompassContext<Stage>
    {

        private readonly CompassClient _client;


        public StageContext(CompassClient client)
        {
            _client = client;
        }

        #region CRUD

        public Stage Get(int id)
        {
            var task = GetAsync(id, CancellationToken.None);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<Stage> GetAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("opportunities/stage/{id}");
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));

            var results = await _client.ExecuteAsync<List<Stage>>(request, cancelToken).ConfigureAwait(false);

            return results.Data.FirstOrDefault();
        }

        public async Task<UpsertResult<Stage>> UpsertAsync(Stage entity, CancellationToken cancelToken,
            int? parentId = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var result = new UpsertResult<Stage>();

            if (entity.StageID > 0)
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

        public async Task<Stage> CreateAsync(Stage entity, CancellationToken cancelToken, int? parentId = null)
        {
            var stages = await CreateAsync(new[] { entity }, cancelToken).ConfigureAwait(false);
            return stages.FirstOrDefault();
        }

        public async Task<List<Stage>> CreateAsync(IEnumerable<Stage> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportuntiies/stage", Method.POST);
            request.AddJsonBody(entities);

            var response = await _client.ExecuteAsync<List<Stage>>(request, cancelToken).ConfigureAwait(false);

            return response.Data;
        }

        public async Task<Stage> UpdateAsync(Stage entity, CancellationToken cancelToken)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var request = _client.NewRequest("opportutnities/stage/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.StageID.ToString(CultureInfo.InvariantCulture));
            request.AddJsonBody(entity);

            var response = await _client.ExecuteAsync<List<Stage>>(request, cancelToken).ConfigureAwait(false);

            return response.Data.FirstOrDefault();
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("opportunities/stage/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));

            await _client.ExecuteAsync(request, cancelToken).ConfigureAwait(false);
        }

        #endregion

        #region Subitems 

        


        #endregion

        #region Changes

        public IList<ChangeEvent> GetChanges(byte[] version = null, bool includeDeleted = false)
        {
            var task = GetChangesAsync(version, includeDeleted, CancellationToken.None);
            task.RunSynchronously();

            return task.Result;
        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            var request = _client.NewRequest("opportunities/stage/changes");
            request.AddQueryParameter("version", Convert.ToBase64String(rowVersion));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());

            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token).ConfigureAwait(false);

            return results.Data;
        }

        #endregion

        #region Metadata

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

        #endregion


    }

}

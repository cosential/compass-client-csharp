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
   public class StaffTeamContext : ICompassContext<StaffTeam>
    {

        private readonly CompassClient _client;


        public StaffTeamContext(CompassClient client)
        {
            _client = client;
        }

        #region CRUD

        public StaffTeam Get(int staffTeamId)
        {
            var task = GetAsync(staffTeamId, CancellationToken.None);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<StaffTeam> GetAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("opportunities/staffteam/{id}");
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));

            var results = await _client.ExecuteAsync<StaffTeam>(request, cancelToken).ConfigureAwait(false);

            return results.Data;
        }

        public async Task<UpsertResult<StaffTeam>> UpsertAsync(StaffTeam entity, CancellationToken cancelToken,
            int? parentId = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var result = new UpsertResult<StaffTeam>();

            if (entity.OppStaffTeamID > 0)
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

        public async Task<StaffTeam> CreateAsync(StaffTeam entity, CancellationToken cancelToken, int? parentId = null)
        {
            var staffTeams = await CreateAsync(new[] { entity }, cancelToken ).ConfigureAwait(false);
            return staffTeams.FirstOrDefault();
        }

        public async Task<List<StaffTeam>> CreateAsync(IEnumerable<StaffTeam> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportunities/staffteam", Method.POST);
            request.AddJsonBody(entities);

            var response = await _client.ExecuteAsync<List<StaffTeam>>(request, cancelToken).ConfigureAwait(false);

            return response.Data;
        }

        public async Task<StaffTeam> UpdateAsync(StaffTeam entity, CancellationToken cancelToken)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var request = _client.NewRequest("opportunities/staffteam/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.OppStaffTeamID.ToString(CultureInfo.InvariantCulture));
            request.AddJsonBody(entity);

            var response = await _client.ExecuteAsync<StaffTeam>(request, cancelToken).ConfigureAwait(false);

            return response.Data;
        }

        public async Task DeleteAsync(long id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("opportunities/staffteam/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));

            await _client.ExecuteAsync(request, cancelToken).ConfigureAwait(false);
        }

        #endregion

        #region Subitems 

        public async Task<List<StaffTeamRole>> GetStaffTeamRoleAsync(int StaffTeamId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportunities/staffteam/staffteamroles/{id}");
            request.AddUrlSegment("id", StaffTeamId.ToString(CultureInfo.InvariantCulture));

            var result = await _client.ExecuteAsync<List<StaffTeamRole>>(request, cancelToken).ConfigureAwait(false);
            return result.Data ?? new List<StaffTeamRole>();
        }


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
            var request = _client.NewRequest("opportunities/staffteam/changes");
            request.AddQueryParameter("includeDeleted", true.ToString());

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

        public Task DeleteAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            return DeleteAsync((long)id, cancelToken, parentId);
        }

        #endregion


    }

}

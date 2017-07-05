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

        public async Task<StaffTeam> GetAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportunities/staffteam/{id}");
            request.AddUrlSegment("id", id.ToString());

            var results = await _client.ExecuteAsync<StaffTeam>(request, cancelToken);

            return results.Data;
        }

        public async Task<UpsertResult<StaffTeam>> UpsertAsync(StaffTeam entity, CancellationToken cancelToken)
        {

            var result = new UpsertResult<StaffTeam>();

            if (entity.OppStaffTeamID > 0)
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

        public async Task<StaffTeam> CreateAsync(StaffTeam entity, CancellationToken cancelToken)
        {
            var staffTeams = await CreateAsync(new[] { entity }, cancelToken );
            return staffTeams.FirstOrDefault();
        }

        public async Task<List<StaffTeam>> CreateAsync(IEnumerable<StaffTeam> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportuntiies/staffteam", Method.POST);
            request.AddBody(entities);

            var response = await _client.ExecuteAsync<List<StaffTeam>>(request, cancelToken);

            return response.Data;
        }

        public async Task<StaffTeam> UpdateAsync(StaffTeam entity, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportutnities/staffteam/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.OppStaffTeamID.ToString());
            request.AddBody(entity);

            var response = await _client.ExecuteAsync<StaffTeam>(request, cancelToken);

            return response.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportunities/staffteam/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());

            await _client.ExecuteAsync(request, cancelToken);
        }

        #endregion

        #region Subitems 

        public async Task<List<StaffTeamRole>> GetStaffTeamRoleAsync(int StaffTeamId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportunities/staffteam/staffteamroles/{id}");
            request.AddUrlSegment("id", StaffTeamId.ToString());

            var result = await _client.ExecuteAsync<List<StaffTeamRole>>(request, cancelToken);
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

            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token);

            return results.Data;
        }

        #endregion

        #region Metadata

        public Task<TM> GetMetadataAync<TM>(MetadataScope scope, int entityId, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(TM));
        }

        public Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(TM));
        }

        #endregion


    }

}

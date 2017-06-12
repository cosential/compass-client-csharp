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

            if (entity.OppStaffTeamID.HasValue && entity.OppStaffTeamID.Value > 0)
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

        #region CRUD

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

        public Task<Dictionary<string, object>> GetMetadataAync(MetadataScope scope, int id, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Dictionary<string, object>());
        }

        public Task<Dictionary<string, object>> PutMetadataAsync(MetadataScope scope, int entityId, Dictionary<String, object> data, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Dictionary<string, object>());
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


    }

}

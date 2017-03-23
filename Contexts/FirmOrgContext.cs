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
    [Obsolete("Each firm org needs its own context because they are different data types")]
    public class FirmOrgContext
    {

        private readonly CompassClient _client;

        public FirmOrgContext(CompassClient client)
        {
            _client = client;
        }

        #region CRUD

        public Task<Office> GetOfficeAsync(int id, CancellationToken cancel)
        {
            return GetAsync<Office>(FirmOrg.Offices, id, cancel);
        }

        public Task<Division> GetDivisionAsync(int id, CancellationToken cancel)
        {
            return GetAsync<Division>(FirmOrg.Divisions, id, cancel);
        }

        public async Task DeleteOfficeAsync(int id, CancellationToken cancel)
        {
            await DeleteAsync(FirmOrg.Offices, id, cancel);
        }

        public async Task DeleteAsync(FirmOrg firmorg, int id, CancellationToken cancel)
        {
            var request = _client.NewRequest($"firmorgs/{firmorg}/{id}", Method.DELETE);
            await _client.ExecuteAsync(request, cancel);

        }

        public async Task<UpsertResult<Office>> UpsertOfficeAsync(Office data, CancellationToken cancel)
        {
            var result = new UpsertResult<Office>();

            if (data.OfficeID.HasValue && data.OfficeID.Value > 0)
            {
                result.Action = UpsertAction.Updated;
                result.Data = await UpdateOfficeAsync(data, cancel);
            }
            else
            {
                result.Action = UpsertAction.Created;
                result.Data = await CreateAsync(data, cancel);
            }

            return result;
        }

        public async Task<Office> UpdateOfficeAsync(Office data, CancellationToken cancel)
        {
            var request = _client.NewRequest($"firmorgs/offices/{data.OfficeID}", Method.PUT);
            request.AddBody(data);

            var results = await _client.ExecuteAsync<Office>(request, cancel);
            return results.Data;
        }

        public async Task<Office> CreateAsync(Office data, CancellationToken cancel)
        {
            var result = await CreateAsync(new[] {data}, cancel);
            return result.FirstOrDefault();
        }

        public async Task<IList<Office>> CreateAsync(IEnumerable<Office> data, CancellationToken cancel)
        {
            var request = _client.NewRequest("firmorgs/offices", Method.POST);

            foreach (var office in data)
            {
                office.OfficeID = office.OfficeID ?? 0;
            }

            request.AddBody(data);

            var results = await _client.ExecuteAsync<List<Office>>(request, cancel);
            return results.Data;
        }

        public T Get<T>(FirmOrg firmorg, int id)
        {
            var task = GetAsync<T>(firmorg, id, CancellationToken.None);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<T> GetAsync<T>(FirmOrg firmorg, int id, CancellationToken cancel)
        {
            var request = _client.NewRequest($"firmorgs/{firmorg}/{id}");
            var results = await _client.ExecuteAsync<T>(request, cancel);

            return results.Data;
        }

        #endregion

        #region FirmOrgsLists

        public IList<Office> ListOffices(int from, int take, string entity, int? entityId)
        {
            var request = _client.NewRequest($"{entity}/{entityId}/offices")
                ;
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", take.ToString());

            var results = _client.Execute<List<Office>>(request);
            return results.Data;

        }

        public IList<Division> ListDivisions(int from, int take, string entity, int? entityId)
        {
            var request = _client.NewRequest($"{entity}/{entityId}/divisions");

            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", take.ToString());

            var results = _client.Execute<List<Division>>(request);
            return results.Data;
        
        }

        public Task<List<ChangeEvent>> GetOfficeChangesAsync(byte[] rowVersion, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IList<OfficeDivision> ListOfficeDivisions(int from, int take, string entity, int? entityId)
        {
            var request = _client.NewRequest($"{entity}/{entityId}/officeDivisions", Method.GET);
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", take.ToString());

            var results = _client.Execute<List<OfficeDivision>>(request);
            return results.Data;

        }

        public IList<Studio> ListStudios(int from, int take, string entity, int? entityId)
        {
            var request = _client.NewRequest($"{entity}/{entityId}/studios", Method.GET);
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", take.ToString());

            var results = _client.Execute<List<Studio>>(request);
            return results.Data;

        }

        public IList<Territory> ListTerritories(int from, int take, string entity, int? entityId)
        {
            var request = _client.NewRequest($"{entity}/{entityId}/territories", Method.GET);
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", take.ToString());

            var results = _client.Execute<List<Territory>>(request);
            return results.Data;

        }

        public IList<PracticeArea> ListPracticeAreas(int from, int take, string entity, int? entityId)
        {
            var request = _client.NewRequest($"{entity}/{entityId}/practiceAreas", Method.GET);
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", take.ToString());

            var results = _client.Execute<List<PracticeArea>>(request);
            return results.Data;

        }

        public IList<ChangeEvent> GetChanges(FirmOrg firmOrg, byte[] version = null, bool includeDeleted = false)
        {
            var task = GetChangesAsync(firmOrg, version, includeDeleted, CancellationToken.None);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<List<ChangeEvent>> GetChangesAsync(FirmOrg firmOrg, byte[] version = null, bool includeDeleted = false, CancellationToken? cancel = null)
        {
            var request = _client.NewRequest("firmorgs/{firmOrg}/changes");
            request.AddUrlSegment("firmOrg", firmOrg.ToString().ToLower());
            if (version != null) request.AddQueryParameter("version", Convert.ToBase64String(version));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel ?? CancellationToken.None);
            return results.Data;
        }

        #endregion
    }
}

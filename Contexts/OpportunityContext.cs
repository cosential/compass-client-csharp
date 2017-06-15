using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public class OpportunityContext : ICompassContext<Opportunity>
    {

        private readonly CompassClient _client;

        public OpportunityContext(CompassClient client )
        {
            _client = client;
        }

        #region CRUD

        public Opportunity Get(int opportunityId)
        {
            var task = GetAsync(opportunityId, CancellationToken.None);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<Opportunity> GetAsync(int opportunityId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportunities/{id}");
            request.AddUrlSegment("id", opportunityId.ToString());

            var results = await _client.ExecuteAsync<Opportunity>(request, cancelToken);
            return results.Data;
        }

        public async Task<Opportunity> UpdateAsync(Opportunity entity, CancellationToken cancel)
        {
            var request = _client.NewRequest("opportunities/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.OpportunityId.ToString());
            request.AddBody(entity);

            var results = await _client.ExecuteAsync<Opportunity>(request, cancel);
            return results.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancel)
        {
            var request = _client.NewRequest("opportunities/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());

            await _client.ExecuteAsync<Opportunity>(request, cancel);
        }

        public async Task<Opportunity> CreateAsync(Opportunity entity, CancellationToken cancel)
        {
            var result = await CreateAsync(new[] { entity }, cancel);
            return result.FirstOrDefault();
        }

        public async Task<IList<Opportunity>> CreateAsync(IEnumerable<Opportunity> entities, CancellationToken cancel)
        {
            var request = _client.NewRequest("opportunities", Method.POST);
            request.AddBody(entities);

            var results = await _client.ExecuteAsync<List<Opportunity>>(request, cancel);
            return results.Data;
        }

        public async Task<UpsertResult<Opportunity>> UpsertAsync(Opportunity entity, CancellationToken cancelToken)
        {
            var result = new UpsertResult<Opportunity>();

            if (entity.OpportunityId.HasValue && entity.OpportunityId.Value > 0)
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

        public List<Opportunity> List(int from, int size, bool full = true)
        {
            var request = _client.NewRequest("opportunities", Method.GET);
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", size.ToString());
            request.AddQueryParameter("full", full.ToString());

            var results = _client.Execute<List<Opportunity>>(request);
            if (results.Data == null)
            {
                Debug.Write(results.Content);
            }
            return results.Data;
        }

        #endregion

        #region Changes

        public IList<ChangeEvent> GetChanges(byte[] version = null, bool includeDeleted = false)
        {
            var task = GetChangesAsync(version, includeDeleted, CancellationToken.None);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] version, bool includeDeleted, CancellationToken cancel)
        {
            var request = _client.NewRequest("opportunities/changes");
            if (version != null) request.AddQueryParameter("version", Convert.ToBase64String(version));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel);
            return results.Data;
        }

        #endregion


        #region Subitems
        public List<Office> GetOffices(int opportunityId)
        {
            return _client.GetSubItems<Office>(PrimaryEntityType.Opportunity, opportunityId, "offices");
        }

        public async Task<List<int>> GetFirmOrgIdListAsync(FirmOrg firmorg, int opportunityId, CancellationToken cancel)
        {
            var fc = new FirmOrgContext();

            var request = _client.NewRequest("opportunities/{id}/{firmorg}");
            request.AddUrlSegment("id", opportunityId.ToString());
            request.AddUrlSegment("firmorg", firmorg.ToString());
            request.AddQueryParameter("fields", fc.GetPrimaryKeyName(firmorg));

            switch (firmorg)
            {
                case FirmOrg.Offices:
                    var officeResponse = await _client.ExecuteAsync<List<Office>>(request, cancel);
                    return officeResponse.Data.Where(x => x.OfficeID.HasValue).Select(x => x.OfficeID.Value).ToList();
                case FirmOrg.Divisions:
                    var divisionResponse = await _client.ExecuteAsync<List<Division>>(request, cancel);
                    return divisionResponse.Data.Where(x => x.DivisionID.HasValue).Select(x => x.DivisionID.Value).ToList();
                case FirmOrg.Studios:
                    var studioResponse = await _client.ExecuteAsync<List<Studio>>(request, cancel);
                    return studioResponse.Data.Where(x => x.StudioID.HasValue).Select(x => x.StudioID.Value).ToList();
                case FirmOrg.PracticeAreas:
                    var practiceAreasResponse = await _client.ExecuteAsync<List<PracticeArea>>(request, cancel);
                    return practiceAreasResponse.Data.Where(x => x.PracticeAreaID.HasValue).Select(x => x.PracticeAreaID.Value).ToList();
                case FirmOrg.Territories:
                    var territoryResponse = await _client.ExecuteAsync<List<Territory>>(request, cancel);
                    return territoryResponse.Data.Where(x => x.TerritoryID.HasValue).Select(x => x.TerritoryID.Value).ToList();
                default:
                    throw new ArgumentOutOfRangeException(nameof(firmorg), firmorg, null);
            }

            
        }

        public async Task SetFirmOrgIdListAsync(FirmOrg firmorg, int opportunityId, List<int> idList, CancellationToken cancel)
        {
            var oldIdList = await GetFirmOrgIdListAsync(firmorg, opportunityId, cancel);
            var removeIdList = oldIdList.Where(i => !idList.Contains(i)).ToList();
            var addIdList = idList.Where(i => !oldIdList.Contains(i)).ToList();

            foreach (var i in removeIdList)
            {
                var request = _client.NewRequest("opportunities/{id}/{firmorg}/{oid}", Method.DELETE);
                request.AddUrlSegment("id", opportunityId.ToString());
                request.AddUrlSegment("firmorg", firmorg.ToString());
                request.AddUrlSegment("oid", i.ToString());
                await _client.ExecuteAsync(request, cancel);
            }

            if (addIdList.Any())
            {
                var fc = new FirmOrgContext();

                var request = _client.NewRequest("opportunities/{id}/{firmorg}", Method.POST);
                request.AddUrlSegment("id", opportunityId.ToString());
                request.AddUrlSegment("firmorg", firmorg.ToString());
                request.AddBody(addIdList.Select(
                    i => new Dictionary<string, int> {{fc.GetPrimaryKeyName(firmorg), i}}));

                await _client.ExecuteAsync(request, cancel);
            }
        }

        public async Task<List<PrimaryCategory>> GetPrimaryCategoriesAsync(int opportunityId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportunities/{id}/primarycategories");
            request.AddUrlSegment("id", opportunityId.ToString());

            var result = await _client.ExecuteAsync<List<PrimaryCategory>>(request, cancelToken);
            return result.Data ?? new List<PrimaryCategory>();
        }

        public async Task<List<StaffTeam>> GetStaffTeamAsync(int opportunityId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportunities/{id}/staffteam");
            request.AddUrlSegment("id", opportunityId.ToString());

            var result = await _client.ExecuteAsync<List<StaffTeam>>(request, cancelToken);
            return result.Data ?? new List<StaffTeam>();
        }

        public async Task<List<SecondaryCategory>> GetSecondaryCategoriesAsync(int opportunityId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportunities/{id}/secondarycategories");
            request.AddUrlSegment("id", opportunityId.ToString());

            var result = await _client.ExecuteAsync<List<SecondaryCategory>>(request, cancelToken);
            return result.Data ?? new List<SecondaryCategory>();
        }

        public async Task<List<DeliveryMethod>> GetDeliveryMethodsAsync(int opportunityId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportunities/{id}/deliverymethod");
            request.AddUrlSegment("id", opportunityId.ToString());

            var result = await _client.ExecuteAsync<List<DeliveryMethod>>(request, cancelToken);
            return result.Data ?? new List<DeliveryMethod>();
        }

        public async Task<List<OpportunityCompany>> GetCompaniesAsync(int opportunityId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("opportunities/{id}/companies");
            request.AddUrlSegment("id", opportunityId.ToString());

            var result = await _client.ExecuteAsync<List<OpportunityCompany>>(request, cancelToken);
            return result.Data ?? new List<OpportunityCompany>();
        }

        public List<Studio> GetStudios(int opportunityId)
        {
            return _client.GetSubItems<Studio>(PrimaryEntityType.Opportunity, opportunityId, "studios");
        }

        public List<OfficeDivision> GetOfficeDivisions(int opportunityId)
        {
            return _client.GetSubItems<OfficeDivision>(PrimaryEntityType.Opportunity, opportunityId, "officedivisions");
        }

        public List<PracticeArea> GetPracticeAreas(int opportunityId)
        {
            return _client.GetSubItems<PracticeArea>(PrimaryEntityType.Opportunity, opportunityId, "practiceareas");
        }

        public List<Territory> GetTerritories(int opportunityId)
        {
            return _client.GetSubItems<Territory>(PrimaryEntityType.Opportunity, opportunityId, "territories");
        }

        #endregion

        public async Task<Dictionary<string, object>> GetMetadataAync(MetadataScope scope, int id, CancellationToken cancellationToken)
        {
            var request = _client.NewRequest("opportunities/{id}/metadata/{scope}");
            request.AddUrlSegment("id", id.ToString());
            request.AddUrlSegment("scope", scope.ToString());

            var result = await _client.ExecuteAsync<Dictionary<string, object>>(request, cancellationToken);
            return result.Data ?? new Dictionary<string, object>();
        }

        public async Task<Dictionary<string, object>> PutMetadataAsync(MetadataScope scope, int entityId, Dictionary<string, object> data, CancellationToken cancellationToken)
        {
            var request = _client.NewRequest("opportunities/{id}/metadata/{scope}", Method.PUT);
            request.AddUrlSegment("id", entityId.ToString());
            request.AddUrlSegment("scope", scope.ToString());
            request.AddBody(data);

            var result = await _client.ExecuteAsync<Dictionary<string, object>>(request, cancellationToken);
            return result.Data ?? new Dictionary<string, object>();
        }
    }
}

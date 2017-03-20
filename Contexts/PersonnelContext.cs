using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public class PersonnelContext
    {
        private readonly CompassClient _client;
        public PersonnelContext(CompassClient client)
        {
            _client = client;
        }

        #region CRUD

        public IList<Personnel> Create(IEnumerable<Personnel> personnel)
        {
            var request = _client.NewRequest("personnel", Method.POST);
            request.AddBody(personnel);

            var results = _client.Execute<List<Personnel>>(request);
            return results.Data;
        }

        public async Task<IList<Personnel>> CreateAsync(IEnumerable<Personnel> personnel, CancellationToken cancel)
        {
            var request = _client.NewRequest("personnel", Method.POST);
            request.AddBody(personnel);

            var results = await _client.ExecuteAsync<List<Personnel>>(request, cancel);
            return results.Data;
        }

        public Personnel Create(Personnel personnel)
        {
            return Create(new[] { personnel }).FirstOrDefault();
        }

        public async Task<Personnel> CreateAsync(Personnel personnel, CancellationToken cancel)
        {
            var result = await CreateAsync(new [] {personnel}, cancel);
            return result.FirstOrDefault();
        }

        public Personnel Get(int personnelId)
        {
            var request = _client.NewRequest("personnel/{id}", Method.GET);
            request.AddUrlSegment("id", personnelId.ToString());

            var results = _client.Execute<Personnel>(request);
            return results.Data;
        }

        public async Task<Personnel> GetAsync(int personnelId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("personnel/{id}", Method.GET);
            request.AddUrlSegment("id", personnelId.ToString());

            var results = await _client.ExecuteAsync<Personnel>(request, cancelToken);
            return results.Data;
        }

        public IList<Personnel> List(int from, int take)
        {
            var request = _client.NewRequest("personnel", Method.GET);
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("take", take.ToString());

            var results = _client.Execute<List<Personnel>>(request);
            return results.Data;
        }

        public IList<ChangeEvent> GetChanges(byte[] version)
        {
            var request = _client.NewRequest("personnel/changes/{version}", Method.GET);
            request.AddUrlSegment("version", Convert.ToBase64String(version));
            var results = _client.Execute<List<ChangeEvent>>(request);
            return results.Data;
        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] version, CancellationToken cancel)
        {
            var request = _client.NewRequest("personnel/changes/{version}", Method.GET);
            request.AddUrlSegment("version", Convert.ToBase64String(version));
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel);
            return results.Data;
        }

        public Personnel Update(Personnel personnel)
        {
            var request = _client.NewRequest("personnel/{id}", Method.PUT);
            request.AddUrlSegment("id", personnel.PersonnelId.ToString());
            request.AddBody(personnel);

            var results = _client.Execute<Personnel>(request);
            return results.Data;
        }

        public async Task<Personnel> UpdateAsync(Personnel personnel, CancellationToken cancel)
        {
            var request = _client.NewRequest("personnel/{id}", Method.PUT);
            request.AddUrlSegment("id", personnel.PersonnelId.ToString());
            request.AddBody(personnel);

            var results = await _client.ExecuteAsync<Personnel>(request, cancel);
            return results.Data;
        }

        public void Delete(int personnelId)
        {
            var request = _client.NewRequest("personnel/{id}", Method.DELETE);
            request.AddUrlSegment("id", personnelId.ToString());

            var results = _client.Execute<Personnel>(request);
        }

        #endregion

        public IEnumerable<PersonnelImageMetadata> GetPersonnelImageData(Personnel personnel)
        {
            var request = _client.NewRequest("personnel/{id}/images", Method.GET);
            request.AddUrlSegment("id", personnel.PersonnelId.ToString());
            var result = _client.Execute<List<PersonnelImageMetadata>>(request);
            return result.Data;
        }

        public bool HasImage(Personnel personnel)
        {
            return GetPersonnelImageData(personnel).Any();
        }

        public bool UploadImage(Personnel personnel, string photoUrl)
        {
            if (string.IsNullOrWhiteSpace(photoUrl) || HasImage(personnel)) return false;

            var request = _client.NewRequest("/images/personnel/{id}", Method.POST);
            request.AddUrlSegment("id", personnel.PersonnelId.ToString());
            request.AddQueryParameter("defaultImage", "true");
            request.AddQueryParameter("url", photoUrl);
            request.AddHeader("Content-Type", "application/json");

            var result = _client.Execute(request);
            return result.ResponseStatus == ResponseStatus.Completed;
        }

        #region SEARCH

        public List<Personnel> Search(string query, int from = 0, int take = 50)
        {
            var request = _client.NewRequest("personnel/search");
            request.AddQueryParameter("q", query);
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("take", take.ToString());

            var results = _client.Execute<List<Personnel>>(request);
            return results.Data;
        }

        #endregion

        #region convenience functions

        public Personnel GetByExternalId(string externalId)
        {
            var items = Search($"ExternalId.raw:{externalId}");
            return items.FirstOrDefault(p => p.ExternalId == externalId);
        }

        public UpsertResult<Personnel> Upsert(Personnel personnel)
        {
            return new UpsertResult<Personnel>
            {
                Action = (personnel.PersonnelId.HasValue && personnel.PersonnelId.Value > 0) ? UpsertAction.Updated : UpsertAction.Created,
                Data = (personnel.PersonnelId.HasValue && personnel.PersonnelId.Value > 0) ? Update(personnel) : Create(personnel)
            };
        }

        public async Task<UpsertResult<Personnel>> UpsertAsync(Personnel personnel, CancellationToken cancel)
        {
            var result = new UpsertResult<Personnel>();

            if (personnel.PersonnelId.HasValue && personnel.PersonnelId.Value > 0)
            {
                result.Action = UpsertAction.Updated;
                result.Data = await UpdateAsync(personnel, cancel);
            }
            else
            {
                result.Action = UpsertAction.Created;
                result.Data = await CreateAsync(personnel, cancel);
            }

            return result;
        }

        public List<Office> AddOfficeToPersonnel(int personnelId, string officeName)
        {
            //Data to post
            var data = new List<Office>();

            //Look up office
            var findOfficeRequest = _client.NewRequest($"firmorgs/offices");
            findOfficeRequest.AddQueryParameter("q", $"OfficeName.raw:{officeName}");
            var findOfficeResults = _client.Execute<List<Office>>(findOfficeRequest);

            if (findOfficeResults.Data.Any())
            {
                data.Add(findOfficeResults.Data.First());
            }
            else
            {
                //Add new office
                var addOfficeRequest = _client.NewRequest($"firmorgs/offices", Method.POST);
                addOfficeRequest.AddBody(new List<Office> { new Office { OfficeName = officeName } });
                var addOfficeResponse = _client.Execute<List<Office>>(addOfficeRequest);
                if (addOfficeResponse.Data.Any()) data.Add(addOfficeResponse.Data.First());
                else throw new Exception($"Could not find or create an office named {officeName} in Cosential");
            }

            //Associate the office to the personnel
            var request = _client.NewRequest($"personnel/{personnelId}/offices", Method.POST);
            request.AddBody(data);
            var results = _client.Execute<List<Office>>(request);
            return results.Data;
        }

        public UpsertResult<Personnel> UpsertByExternalId(Personnel personnel)
        {
            var found = GetByExternalId(personnel.ExternalId);
            if (found != null) personnel.PersonnelId = found.PersonnelId;

            return Upsert(personnel);
        }

        #endregion
    }
}

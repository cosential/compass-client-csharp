﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public class PersonnelContext : ICompassContext<Personnel>
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
            request.AddJsonBody(personnel);

            var results = _client.Execute<List<Personnel>>(request);
            return results.Data;
        }

        public async Task<IList<Personnel>> CreateAsync(IEnumerable<Personnel> personnel, CancellationToken cancel)
        {
            var request = _client.NewRequest("personnel", Method.POST);
            request.AddJsonBody(personnel);

            var results = await _client.ExecuteAsync<List<Personnel>>(request, cancel).ConfigureAwait(false);
            return results.Data;
        }

        public Personnel Create(Personnel personnel)
        {
            return Create(new[] { personnel }).FirstOrDefault();
        }

        public async Task<Personnel> CreateAsync(Personnel personnel, CancellationToken cancel, int? parentId = null)
        {
            var result = await CreateAsync(new [] {personnel}, cancel).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public Personnel Get(int personnelId)
        {
            var request = _client.NewRequest("personnel/{id}");
            request.AddUrlSegment("id", personnelId.ToString(CultureInfo.InvariantCulture));

            var results = _client.Execute<Personnel>(request);
            return results.Data;
        }

        public async Task<Personnel> GetAsync(int personnelId, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("personnel/{id}");
            request.AddUrlSegment("id", personnelId.ToString(CultureInfo.InvariantCulture));

            var results = await _client.ExecuteAsync<Personnel>(request, cancelToken).ConfigureAwait(false);
            return results.Data;
        }

        public IList<Personnel> List(int from, int take)
        {
            var request = _client.NewRequest("personnel");
            request.AddQueryParameter("from", from.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("take", take.ToString(CultureInfo.InvariantCulture));

            var results = _client.Execute<List<Personnel>>(request);
            return results.Data;
        }

        public IList<ChangeEvent> GetChanges(byte[] version = null, bool includeDeleted = false)
        {
            var task = GetChangesAsync(version, includeDeleted, CancellationToken.None);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] version, bool includeDeleted, CancellationToken cancel)
        {
            var request = _client.NewRequest("personnel/changes");
            if (version != null) request.AddQueryParameter("version", Convert.ToBase64String(version));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel).ConfigureAwait(false);
            return results.Data;
        }

        public Personnel Update(Personnel personnel)
        {
            if (personnel == null) throw new ArgumentNullException(nameof(personnel));
            var request = _client.NewRequest("personnel/{id}", Method.PUT);
            request.AddUrlSegment("id", personnel.PersonnelId.ToString());
            request.AddJsonBody(personnel);

            var results = _client.Execute<Personnel>(request);
            return results.Data;
        }

        public async Task<Personnel> UpdateAsync(Personnel personnel, CancellationToken cancel)
        {
            if (personnel == null) throw new ArgumentNullException(nameof(personnel));
            var request = _client.NewRequest("personnel/{id}", Method.PUT);
            request.AddUrlSegment("id", personnel.PersonnelId.ToString());
            request.AddJsonBody(personnel);

            var results = await _client.ExecuteAsync<Personnel>(request, cancel).ConfigureAwait(false);
            return results.Data;
        }

        public void Delete(int personnelId)
        {
            var request = _client.NewRequest("personnel/{id}", Method.DELETE);
            request.AddUrlSegment("id", personnelId.ToString(CultureInfo.InvariantCulture));

            var results = _client.Execute<Personnel>(request);
        }

        public async Task DeleteAsync(int personnelId, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("personnel/{id}", Method.DELETE);
            request.AddUrlSegment("id", personnelId.ToString(CultureInfo.InvariantCulture));

            await _client.ExecuteAsync(request, cancelToken).ConfigureAwait(false);
        }

        #endregion

        public IEnumerable<PersonnelImageMetadata> GetPersonnelImageData(Personnel personnel)
        {
            if(personnel == null) throw new ArgumentNullException(nameof(personnel));

            var request = _client.NewRequest("personnel/{id}/images");
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
            if(personnel == null) throw new ArgumentNullException(nameof(personnel));
            if (string.IsNullOrWhiteSpace(photoUrl) || HasImage(personnel)) return false;

            var request = _client.NewRequest("/images/personnel/{id}", Method.POST);
            request.AddUrlSegment("id", personnel.PersonnelId.ToString());
            request.AddQueryParameter("defaultImage", "true");
            request.AddQueryParameter("url", photoUrl);
            request.AddHeader("Content-Type", "application/json");

            var result = _client.Execute(request);
            return result.ResponseStatus == ResponseStatus.Completed;
        }

        public async Task<TM> GetMetadataAync<TM>(MetadataScope scope, int id, CancellationToken cancellationToken,
            int? parentId = null)
        {
            var request = _client.NewRequest("personnel/{id}/metadata/{scope}");
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("scope", scope.ToString());

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken).ConfigureAwait(false);
            return result.Data;
        }

        public async Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data,
            CancellationToken cancellationToken, int? parentId = null)
        {
            var request = _client.NewRequest("personnel/{id}/metadata/{scope}", Method.PUT);
            request.AddUrlSegment("id", entityId.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("scope", scope.ToString());
            request.AddJsonBody(data);

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken).ConfigureAwait(false);
            return result.Data;
        }

        #region SEARCH

        public List<Personnel> Search(string query, int from = 0, int take = 50)
        {
            var request = _client.NewRequest("personnel/search");
            request.AddQueryParameter("q", query);
            request.AddQueryParameter("from", from.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("take", take.ToString(CultureInfo.InvariantCulture));

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
            if (personnel == null) throw new ArgumentNullException(nameof(personnel));
            return new UpsertResult<Personnel>
            {
                Action = (personnel.PersonnelId.HasValue && personnel.PersonnelId.Value > 0) ? UpsertAction.Updated : UpsertAction.Created,
                Data = (personnel.PersonnelId.HasValue && personnel.PersonnelId.Value > 0) ? Update(personnel) : Create(personnel)
            };
        }

        public async Task<UpsertResult<Personnel>> UpsertAsync(Personnel personnel, CancellationToken cancel,
            int? parentId = null)
        {
            if (personnel == null) throw new ArgumentNullException(nameof(personnel));
            var result = new UpsertResult<Personnel>();

            if (personnel.PersonnelId.HasValue && personnel.PersonnelId.Value > 0)
            {
                result.Action = UpsertAction.Updated;
                result.Data = await UpdateAsync(personnel, cancel).ConfigureAwait(false);
            }
            else
            {
                result.Action = UpsertAction.Created;
                result.Data = await CreateAsync(personnel, cancel).ConfigureAwait(false);
            }

            return result;
        }        

        public UpsertResult<Personnel> UpsertByExternalId(Personnel personnel)
        {
            if (personnel == null) throw new ArgumentNullException(nameof(personnel));
            var found = GetByExternalId(personnel.ExternalId);
            if (found != null) personnel.PersonnelId = found.PersonnelId;

            return Upsert(personnel);
        }

        #endregion

        #region ASSOCIATE SUB-OBJECTS

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
                addOfficeRequest.AddJsonBody(new List<Office> { new Office { OfficeName = officeName } });
                var addOfficeResponse = _client.Execute<List<Office>>(addOfficeRequest);
                if (addOfficeResponse.Data.Any()) data.Add(addOfficeResponse.Data.First());
                else throw new Exception($"Could not find or create an office named {officeName} in Cosential");
            }

            //Associate the office to the personnel
            var request = _client.NewRequest($"personnel/{personnelId}/offices", Method.POST);
            request.AddJsonBody(data);
            var results = _client.Execute<List<Office>>(request);
            return results.Data;
        }

        public IList<PersonnelEducation> Create(int personnelId, IEnumerable<PersonnelEducation> education)
        {
            var request = _client.NewRequest($"personnel/{personnelId}/education", Method.POST);
            request.AddJsonBody(education);
            var results = _client.Execute<List<PersonnelEducation>>(request);
            return results.Data;
        }

        public PersonnelEducation AddEducationToPersonnel(int personnelId, PersonnelEducation education)
        {
            return Create(personnelId, new PersonnelEducation[] { education }).FirstOrDefault();
        }

        public PersonnelEducation Update(int personnelId, PersonnelEducation education)
        {
            if (education == null) throw new ArgumentNullException(nameof(education));
            var request = _client.NewRequest("personnel/{personnelId}/education/{DegreeId}", Method.PUT);
            request.AddUrlSegment("personnelId", personnelId.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("DegreeId", education.DegreeId.ToString());
            request.AddJsonBody(education);

            var results = _client.Execute<PersonnelEducation>(request);
            return results.Data;
        }

        public void DeleteAllEducationRecords(int personnelId)
        {
            var request = _client.NewRequest($"personnel/{personnelId}/education", Method.DELETE);
            _client.Execute<Personnel>(request);
        }

        public void DeleteEducationRecord(int personnelId, int degreeId)
        {
            var request = _client.NewRequest("personnel/{personnelId}/education/{degreeId}", Method.DELETE);
            request.AddUrlSegment("personnelId", personnelId.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("degreeId", degreeId.ToString(CultureInfo.InvariantCulture));

            _client.Execute<Personnel>(request);
        }

        #endregion
    }
}

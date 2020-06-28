using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client
{
    [Obsolete("Use PersonnelContext instead", false)]
    public class PersonnelCompassClient : CompassClient
    {
        public PersonnelCompassClient(int firmId, Guid apiKey, string username, string password, Uri host=null) : base(firmId, apiKey, username, password, host)
        {
        }

        #region CRUD

        public IList<Personnel> Create(IEnumerable<Personnel> personnel)
        {
            var request = NewRequest("personnel", Method.POST);
            request.AddJsonBody(personnel);

            var results = Execute<List<Personnel>>(request);
            return results.Data;
        }

        public Personnel Create(Personnel personnel)
        {
            return Create(new[] { personnel }).FirstOrDefault();
        }

        public Personnel Get(int personnelId)
        {
            var request = NewRequest("personnel/{id}");
            request.AddUrlSegment("id", personnelId.ToString(CultureInfo.InvariantCulture));

            var results = Execute<Personnel>(request);
            return results.Data;
        }

        public IList<Personnel> List(int from, int take)
        {
            var request = NewRequest("personnel");
            request.AddQueryParameter("from", from.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("take", take.ToString(CultureInfo.InvariantCulture));

            var results = Execute<List<Personnel>>(request);
            return results.Data;
        }

        public Personnel Update(Personnel personnel)
        {
            if (personnel == null) throw new ArgumentNullException(nameof(personnel));
            var request = NewRequest("personnel/{id}", Method.PUT);
            request.AddUrlSegment("id", personnel.PersonnelId.ToString());
            request.AddJsonBody(personnel);

            var results = Execute<Personnel>(request);
            return results.Data;
        }

        public void Delete(int personnelId)
        {
            var request = NewRequest("personnel/{id}", Method.DELETE);
            request.AddUrlSegment("id", personnelId.ToString(CultureInfo.InvariantCulture));

            Execute<Personnel>(request);
        }

        #endregion

        public IEnumerable<PersonnelImageMetadata> GetPersonnelImageData(Personnel personnel)
        {
            if(personnel == null) throw new ArgumentNullException(nameof(personnel));
            if (!personnel.PersonnelId.HasValue) throw new ArgumentException("Personnel ID can not be null.");
            var request = NewRequest("personnel/{id}/images");
            request.AddUrlSegment("id", personnel.PersonnelId.Value.ToString(CultureInfo.InvariantCulture));
            var result = Execute<List<PersonnelImageMetadata>>(request);
            return result.Data;
        }

        public bool HasImage(Personnel personnel)
        {
            return GetPersonnelImageData(personnel).Any();
        }

        public bool UploadImage(Personnel personnel, string photoUrl)
        {
            if (personnel == null) throw new ArgumentNullException(nameof(personnel));

            if (string.IsNullOrWhiteSpace(photoUrl) || HasImage(personnel)) return false;

            var request = NewRequest("/images/personnel/{id}", Method.POST);
            request.AddUrlSegment("id", personnel.PersonnelId.ToString());
            request.AddQueryParameter("defaultImage", "true");
            request.AddQueryParameter("url", photoUrl);
            request.AddHeader("Content-Type", "application/json");
            
            var result = Execute(request);
            return result.ResponseStatus == ResponseStatus.Completed;
        }

        #region SEARCH

        public List<Personnel> Search(string query, int from=0,int take=50)
        {
            var request = new RestRequest("personnel/search");
            request.AddQueryParameter("q", query);
            request.AddQueryParameter("from", from.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("take", take.ToString(CultureInfo.InvariantCulture));

            var results = Execute<List<Personnel>>(request);
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

        public List<Office> AddOfficeToPersonnel(int personnelId, string officeName)
        {
            //Data to post
            var data = new List<Office>();

            //Look up office
            var findOfficeRequest = NewRequest($"firmorgs/offices");
            findOfficeRequest.AddQueryParameter("q", $"OfficeName.raw:{officeName}");
            var findOfficeResults = Execute<List<Office>>(findOfficeRequest);

            if (findOfficeResults.Data.Any())
            {
                data.Add(findOfficeResults.Data.First());
            }
            else
            {
                //Add new office
                var addOfficeRequest = NewRequest($"firmorgs/offices", Method.POST);
                addOfficeRequest.AddJsonBody(new List<Office> {new Office {OfficeName = officeName}});
                var addOfficeResponse = Execute<List<Office>>(addOfficeRequest);
                if (addOfficeResponse.Data.Any()) data.Add(addOfficeResponse.Data.First());
                else throw new Exception($"Could not find or create an office named {officeName} in Cosential");
            }

            //Associate the office to the personnel
            var request = NewRequest($"personnel/{personnelId}/offices", Method.POST);
            request.AddJsonBody(data);
            var results = Execute<List<Office>>(request);
            return results.Data;
        }

        public UpsertResult<Personnel> UpsertByExternalId(Personnel personnel)
        {
            if (personnel == null) throw new ArgumentNullException(nameof(personnel));
            var found = GetByExternalId(personnel.ExternalId);
            if (found != null) personnel.PersonnelId = found.PersonnelId;

            return Upsert(personnel);
        }

        #endregion
    }
}

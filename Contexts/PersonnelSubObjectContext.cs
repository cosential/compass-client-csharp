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
    public class PersonnelSubObjectContext : CompassClient
    {
        public PersonnelSubObjectContext(int firmId, Guid apiKey, string username, string password, Uri host = null) : base(firmId, apiKey, username, password, host)
        {

        }

        #region CRUD

        public IList<PersonnelEducation> Create(int personnelId, IEnumerable<PersonnelEducation> education)
        {
            var request = NewRequest($"personnel/{personnelId}/education", Method.POST);
            request.AddBody(education);
            var results = Execute<List<PersonnelEducation>>(request);
            return results.Data;
        }

        public PersonnelEducation Create(int personnelId, PersonnelEducation education)
        {
            return Create(personnelId, new PersonnelEducation[] { education }).FirstOrDefault();
        }

        public PersonnelEducation Update(int personnelId, PersonnelEducation education)
        {
            var request = NewRequest("personnel/{personnelId}/education/{DegreeId}", Method.PUT);
            request.AddUrlSegment("personnelId", personnelId.ToString());
            request.AddUrlSegment("DegreeId", education.DegreeId.ToString());
            request.AddBody(education);

            var results = Execute<PersonnelEducation>(request);
            return results.Data;
        }

        public void DeleteAllEducationRecords(int personnelId)
        {
            var request = NewRequest($"personnel/{personnelId}/education", Method.DELETE);
            
            Execute<Personnel>(request);
        }

        public void DeleteEducationRecord(int personnelId, int degreeId)
        {
            var request = NewRequest("personnel/{personnelId}/education/{degreeId}", Method.DELETE);
            request.AddUrlSegment("personnelId", personnelId.ToString());
            request.AddUrlSegment("degreeId", degreeId.ToString());

            Execute<Personnel>(request);
        }

        #endregion
    }
}

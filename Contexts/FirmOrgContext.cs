using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public class FirmOrgContext
    {

        private readonly CompassClient _client;

        public FirmOrgContext(CompassClient client)
        {
            _client = client;
        }

        #region CRUD

        #endregion

        #region FirmOrgsLists

        public IList<Office> ListOffices(int from, int take, string entity, int? entityId)
        {
            var request = _client.NewRequest($"{entity}/{entityId}/offices", Method.GET)
                ;
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", take.ToString());

            var results = _client.Execute<List<Office>>(request);
            return results.Data;

        }

        public IList<Division> ListDivisions(int from, int take, string entity, int? entityId)
        {
            var request = _client.NewRequest($"{entity}/{entityId}/divisions", Method.GET);
                request.AddQueryParameter("from", from.ToString());
                request.AddQueryParameter("size", take.ToString());

            var results = _client.Execute<List<Division>>(request);
            return results.Data;
        
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

        #endregion
    }
}

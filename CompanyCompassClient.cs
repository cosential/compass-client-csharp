using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client
{
    public class CompanyCompassClient : CompassClient
    {

        public CompanyCompassClient(int firmId, Guid apiKey, string username, string password, Uri host=null) : base(firmId, apiKey, username, password)
        {
        }

        #region CRUD

        public Company Get(int CompanyId)
        {
            var request = new RestRequest("companies/{id}", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("id", CompanyId.ToString());

            var results = Execute<Company>(request);
            return results.Data;
        }

        public IList<Company> List(int from, int take)
        {
            var request = new RestRequest("companies", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("take", take.ToString());

            var results = Execute<List<Company>>(request);
            return results.Data;
        }

        #endregion

    }
}

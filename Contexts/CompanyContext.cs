using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Contexts
{
    public class CompanyContext
    {
        private readonly CompassClient _client;
        public CompanyContext(CompassClient client)
        {
            _client = client;
        }

        #region CRUD

        public Company Get(int companyId)
        {
            var request = _client.NewRequest("companies/{id}", Method.GET);
            request.AddUrlSegment("id", companyId.ToString());

            var results = _client.Execute<Company>(request);
            return results.Data;
        }

        public List<Company> List(int from, int size, bool full = true)
        {
            var request = _client.NewRequest("companies", Method.GET);
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", size.ToString());
            request.AddQueryParameter("full", full.ToString());

            var results = _client.Execute<List<Company>>(request);
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

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] version = null, bool includeDeleted = false, CancellationToken? cancel = null)
        {
            var request = _client.NewRequest("companies/changes");
            if (version != null) request.AddQueryParameter("version", Convert.ToBase64String(version));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel ?? CancellationToken.None);
            return results.Data;
        }

        #endregion

        #region Subitems

        public List<CompanyAddress> GetAddresses(int companyId)
        {
            return _client.GetSubItems<CompanyAddress>(PrimaryEntityType.Company, companyId, "addresses");
        }

        public List<CompanyType> GetTypes(int companyId)
        {
            return _client.GetSubItems<CompanyType>(PrimaryEntityType.Company, companyId, "companytypes");
        }

        public List<Studio> GetStudios(int companyId)
        {
            return _client.GetSubItems<Studio>(PrimaryEntityType.Company, companyId, "studios");
        }

        public List<PracticeArea> GetPracticeAreas(int companyId)
        {
            return _client.GetSubItems<PracticeArea>(PrimaryEntityType.Company, companyId, "practiceareas");
        }

        #endregion

    }
}

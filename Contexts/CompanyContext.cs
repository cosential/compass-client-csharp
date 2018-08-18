using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client;
using Cosential.Integrations.Compass.Client.Contexts;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Contexts
{
    public class CompanyContext : ICompassContext<Company>
    {
        private readonly CompassClient _client;
        public CompanyContext(CompassClient client)
        {
            _client = client;
        }

        #region CRUD

        public Company Get(int companyId)
        {
            var task = GetAsync(companyId, CancellationToken.None);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<Company> GetAsync(int companyId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("companies/{id}");
            request.AddUrlSegment("id", companyId.ToString());

            var results = await _client.ExecuteAsync<Company>(request, cancelToken);
            return results.Data;
        }

        public async Task<Company> UpdateAsync(Company entity, CancellationToken cancel)
        {
            var request = _client.NewRequest("companies/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.CompanyId.ToString());
            request.AddBody(entity);

            var results = await _client.ExecuteAsync<Company>(request, cancel);
            return results.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancel)
        {
            var request = _client.NewRequest("companies/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());
            
            await _client.ExecuteAsync<Company>(request, cancel);
        }

        public async Task<Company> CreateAsync(Company entity, CancellationToken cancel)
        {
            var result = await CreateAsync(new[] { entity }, cancel);
            return result.FirstOrDefault();
        }

        public async Task<IList<Company>> CreateAsync(IEnumerable<Company> entities, CancellationToken cancel)
        {
            var request = _client.NewRequest("companies", Method.POST);
            request.AddBody(entities);

            var results = await _client.ExecuteAsync<List<Company>>(request, cancel);
            return results.Data;
        }

        public async Task<UpsertResult<Company>> UpsertAsync(Company entity, CancellationToken cancelToken)
        {
            var result = new UpsertResult<Company>();

            if (entity.CompanyId.HasValue && entity.CompanyId.Value > 0)
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

        public List<Company> List(int from, int size, bool full = true)
        {
            var request = _client.NewRequest("companies");
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

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] version, bool includeDeleted, CancellationToken cancel)
        {
            var request = _client.NewRequest("companies/changes");
            if (version != null) request.AddQueryParameter("version", Convert.ToBase64String(version));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel);
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

        public async Task<TM> GetMetadataAync<TM>(MetadataScope scope, int id, CancellationToken cancellationToken)
        {
            var request = _client.NewRequest("companies/{id}/metadata/{scope}");
            request.AddUrlSegment("id", id.ToString());
            request.AddUrlSegment("scope", scope.ToString());

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken);
            return result.Data;
        }

        public async Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data, CancellationToken cancellationToken)
        {
            var request = _client.NewRequest("companies/{id}/metadata/{scope}", Method.PUT);
            request.AddUrlSegment("id", entityId.ToString());
            request.AddUrlSegment("scope", scope.ToString());
            request.AddBody(data);

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken);
            return result.Data;
        }

        #endregion

    }
}

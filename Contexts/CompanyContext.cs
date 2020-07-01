using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

        public async Task<Company> GetAsync(int companyId, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("companies/{id}");
            request.AddUrlSegment("id", companyId.ToString(CultureInfo.InvariantCulture));

            var results = await _client.ExecuteAsync<Company>(request, cancelToken).ConfigureAwait(false);
            return results.Data;
        }

        public async Task<Company> UpdateAsync(Company entity, CancellationToken cancel)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var request = _client.NewRequest("companies/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.CompanyId.ToString());
            request.AddJsonBody(entity);

            var results = await _client.ExecuteAsync<Company>(request, cancel).ConfigureAwait(false);
            return results.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancel, int? parentId = null)
        {
            var request = _client.NewRequest("companies/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
            
            await _client.ExecuteAsync<Company>(request, cancel).ConfigureAwait(false);
        }

        public async Task<Company> CreateAsync(Company entity, CancellationToken cancel, int? parentId = null)
        {
            var result = await CreateAsync(new[] { entity }, cancel).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public async Task<IList<Company>> CreateAsync(IEnumerable<Company> entities, CancellationToken cancel)
        {
            var request = _client.NewRequest("companies", Method.POST);
            request.AddJsonBody(entities);

            var results = await _client.ExecuteAsync<List<Company>>(request, cancel).ConfigureAwait(false);
            return results.Data;
        }

        public async Task<UpsertResult<Company>> UpsertAsync(Company entity, CancellationToken cancelToken,
            int? parentId = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var result = new UpsertResult<Company>();

            if (entity.CompanyId.HasValue && entity.CompanyId.Value > 0)
            {
                result.Action = UpsertAction.Updated;
                result.Data = await UpdateAsync(entity, cancelToken).ConfigureAwait(false);
            }
            else
            {
                result.Action = UpsertAction.Created;
                result.Data = await CreateAsync(entity, cancelToken).ConfigureAwait(false);
            }

            return result;
        }

        public List<Company> List(int from, int size, bool full = true)
        {
            var request = _client.NewRequest("companies");
            request.AddQueryParameter("from", from.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("size", size.ToString(CultureInfo.InvariantCulture));
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
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel).ConfigureAwait(false);
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

        public async Task<TM> GetMetadataAync<TM>(MetadataScope scope, int id, CancellationToken cancellationToken,
            int? parentId = null)
        {
            var request = _client.NewRequest("companies/{id}/metadata/{scope}");
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("scope", scope.ToString());

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken).ConfigureAwait(false);
            return result.Data;
        }

        public async Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data,
            CancellationToken cancellationToken, int? parentId = null)
        {
            var request = _client.NewRequest("companies/{id}/metadata/{scope}", Method.PUT);
            request.AddUrlSegment("id", entityId.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("scope", scope.ToString());
            request.AddJsonBody(data);

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken).ConfigureAwait(false);
            return result.Data;
        }

        #endregion

    }
}

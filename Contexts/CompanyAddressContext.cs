using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public class CompanyAddressContext : ICompassContext<CompanyAddress>
    {
        private readonly CompassClient _client;
        public CompanyAddressContext(CompassClient client)
        {
            _client = client;
        }

        #region CRUD

        public CompanyAddress Get(int companyId, int addressId)
        {
            var task = GetAsync(addressId, CancellationToken.None, companyId);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<CompanyAddress> GetAsync(int addressId, CancellationToken cancelToken, int? parentId = null)
        {
            if(!parentId.HasValue) throw new ArgumentException("Parent id value is required to get company address.");

            var request = _client.NewRequest("companies/{id}/addresses/{addressId}");
            request.AddUrlSegment("id", parentId.Value);
            request.AddUrlSegment("addressId", addressId);

            var results = await _client.ExecuteAsync<CompanyAddress>(request, cancelToken);
            return results.Data;
        }

        // todo : remove parent id from signature
        public async Task<CompanyAddress> UpdateAsync(CompanyAddress entity, CancellationToken cancel)
        {
            var request = _client.NewRequest("companies/{id}/addresses/{addressId}", Method.PUT);
            request.AddUrlSegment("id", entity.CompanyId.ToString());
            request.AddUrlSegment("addressId", entity.AddressID);
            request.AddBody(new []{entity});

            var results = await _client.ExecuteAsync<List<CompanyAddress>>(request, cancel);
            return results.Data.FirstOrDefault();
        }

        public async Task DeleteAsync(int id, CancellationToken cancel, int? parentId = null)
        {
            if(!parentId.HasValue) throw new ArgumentException("Parent Id is required to delete company addresss.");
            var request = _client.NewRequest("companies/{id}/addresses/{AddressId}", Method.DELETE);
            request.AddUrlSegment("id", id);
            request.AddUrlSegment("AddressId", parentId.Value);

            await _client.ExecuteAsync<CompanyAddress>(request, cancel);
        }


        public async Task<CompanyAddress> CreateAsync(CompanyAddress entity, CancellationToken cancel,
            int? parentId = null)
        {
            if (!parentId.HasValue) throw new ArgumentException("Parent id value is required to create company address.");

            var result = await CreateAsync(new[] { entity }, cancel, parentId.Value);
            return result.FirstOrDefault();
        }

        public async Task<IList<CompanyAddress>> CreateAsync(IEnumerable<CompanyAddress> entities, CancellationToken cancel, int parentId)
        {
            var request = _client.NewRequest("companies/{companyId}/addresses", Method.POST);
            request.AddUrlSegment("companyId", parentId);
            request.AddBody(entities);

            var results = await _client.ExecuteAsync<List<CompanyAddress>>(request, cancel);
            return results.Data;
        }

        public async Task<UpsertResult<CompanyAddress>> UpsertAsync(CompanyAddress entity,
            CancellationToken cancelToken, int? parentId = null)
        {
            var result = new UpsertResult<CompanyAddress>();

            if (entity.AddressID > 0)
            {
                result.Action = UpsertAction.Updated;
                result.Data = await UpdateAsync(entity, cancelToken);
            }
            else
            {
                result.Action = UpsertAction.Created;
                result.Data = await CreateAsync(entity, cancelToken, parentId);
            }

            return result;
        }

        public List<CompanyAddress> List(int companyId, int from, int size)
        {
            var request = _client.NewRequest("companies/{companyId}/addresses");
            request.AddUrlSegment("companyId", companyId);
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", size.ToString());

            var results = _client.Execute<List<CompanyAddress>>(request);
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
            var request = _client.NewRequest("companies/addresses/changes");
            if (version != null) request.AddQueryParameter("version", Convert.ToBase64String(version));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel);
            return results.Data;
        }

        #endregion

        #region Subitems

       

        public async Task<TM> GetMetadataAync<TM>(MetadataScope scope, int id, CancellationToken cancellationToken,
            int? parentId = null)
        {
            var request = _client.NewRequest("companies/{id}/metadata/{scope}");
            request.AddUrlSegment("id", id.ToString());
            request.AddUrlSegment("scope", scope.ToString());

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken);
            return result.Data;
        }

        public async Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data,
            CancellationToken cancellationToken, int? parentId = null)
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

            var results = await _client.ExecuteAsync<CompanyAddress>(request, cancelToken).ConfigureAwait(false);
            return results.Data;
        }

        // todo : remove parent id from signature
        public async Task<CompanyAddress> UpdateAsync(CompanyAddress entity, CancellationToken cancel)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var request = _client.NewRequest("companies/{id}/addresses/{addressId}", Method.PUT);
            request.AddUrlSegment("id", entity.CompanyId.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("addressId", entity.AddressID);
            request.AddJsonBody(new []{entity});

            var results = await _client.ExecuteAsync<List<CompanyAddress>>(request, cancel).ConfigureAwait(false);
            return results.Data.FirstOrDefault();
        }

        public async Task DeleteAsync(int id, CancellationToken cancel, int? parentId = null)
        {
            if(!parentId.HasValue) throw new ArgumentException("Parent Id is required to delete company addresss.");
            var request = _client.NewRequest("companies/{id}/addresses/{AddressId}", Method.DELETE);
            request.AddUrlSegment("id", id);
            request.AddUrlSegment("AddressId", parentId.Value);

            await _client.ExecuteAsync<CompanyAddress>(request, cancel).ConfigureAwait(false);
        }


        public async Task<CompanyAddress> CreateAsync(CompanyAddress entity, CancellationToken cancel,
            int? parentId = null)
        {
            if (!parentId.HasValue) throw new ArgumentException("Parent id value is required to create company address.");

            var result = await CreateAsync(new[] { entity }, parentId.Value, cancel).ConfigureAwait(false);
            return result.FirstOrDefault();
        }
        
        [Obsolete("Use public async Task<IList<CompanyAddress>> CreateAsync(IEnumerable<CompanyAddress> entities, int parentId, CancellationToken cancel)",false)]
        #pragma warning disable CA1068 // CancellationToken parameters must come last
        public async Task<IList<CompanyAddress>> CreateAsync(IEnumerable<CompanyAddress> entities, CancellationToken cancel, int parentId)
        {
            return await CreateAsync(entities, parentId, cancel).ConfigureAwait(false);
        }
        #pragma warning restore CA1068 // CancellationToken parameters must come last

        public async Task<IList<CompanyAddress>> CreateAsync(IEnumerable<CompanyAddress> entities, int parentId, CancellationToken cancel)
        {
            var request = _client.NewRequest("companies/{companyId}/addresses", Method.POST);
            request.AddUrlSegment("companyId", parentId);
            request.AddJsonBody(entities);

            var results = await _client.ExecuteAsync<List<CompanyAddress>>(request, cancel).ConfigureAwait(false);
            return results.Data;
        }

        public async Task<UpsertResult<CompanyAddress>> UpsertAsync(CompanyAddress entity,
            CancellationToken cancelToken, int? parentId = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var result = new UpsertResult<CompanyAddress>();

            if (entity.AddressID > 0)
            {
                result.Action = UpsertAction.Updated;
                result.Data = await UpdateAsync(entity, cancelToken).ConfigureAwait(false);
            }
            else
            {
                result.Action = UpsertAction.Created;
                result.Data = await CreateAsync(entity, cancelToken, parentId).ConfigureAwait(false);
            }

            return result;
        }

        public List<CompanyAddress> List(int companyId, int from, int size)
        {
            var request = _client.NewRequest("companies/{companyId}/addresses");
            request.AddUrlSegment("companyId", companyId);
            request.AddQueryParameter("from", from.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("size", size.ToString(CultureInfo.InvariantCulture));

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
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel).ConfigureAwait(false);
            return results.Data;
        }

        #endregion

        #region Subitems

       

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

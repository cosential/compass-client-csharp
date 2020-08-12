using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public class ContactAddressContext : ICompassContext<ContactAddress>
    {
        private readonly CompassClient _client;
        public ContactAddressContext(CompassClient client)
        {
            _client = client;
        }

        #region CRUD

        public ContactAddress Get(int contactId, int addressId)
        {
            var task = GetAsync(addressId, CancellationToken.None, contactId);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<ContactAddress> GetAsync(int addressId, CancellationToken cancelToken, int? parentId = null)
        {
            if (!parentId.HasValue) throw new ArgumentException("Parent id value is required to get contact address.");

            var request = _client.NewRequest("contacts/{id}/addresses/{addressId}");
            request.AddUrlSegment("id", parentId.Value);
            request.AddUrlSegment("addressId", addressId);

            var results = await _client.ExecuteAsync<ContactAddress>(request, cancelToken).ConfigureAwait(false);
            return results.Data;
        }

        public async Task<ContactAddress> UpdateAsync(ContactAddress entity, CancellationToken cancel)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var request = _client.NewRequest("contacts/{id}/addresses/{addressId}", Method.PUT);
            request.AddUrlSegment("id", entity.ContactId.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("addressId", entity.AddressID);
            request.AddJsonBody(entity);

            var results = await _client.ExecuteAsync<ContactAddress>(request, cancel).ConfigureAwait(false);
            return results.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancel, int? parentId = null)
        {
            if (!parentId.HasValue) throw new ArgumentException("Parent Id is required to delete contact addresss.");
            var request = _client.NewRequest("contacts/{id}/addresses/{AddressId}", Method.DELETE);
            request.AddUrlSegment("id", id);
            request.AddUrlSegment("AddressId", parentId.Value);

            await _client.ExecuteAsync<ContactAddress>(request, cancel).ConfigureAwait(false);
        }


        public async Task<ContactAddress> CreateAsync(ContactAddress entity, CancellationToken cancel,
            int? parentId = null)
        {
            if (!parentId.HasValue) throw new ArgumentException("Parent id value is required to create contact address.");

            var result = await CreateAsync(new[] { entity },parentId.Value, cancel).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        #pragma warning disable CA1068
        [Obsolete("User public async Task<IList<ContactAddress>> CreateAsync(IEnumerable<ContactAddress> entities, int parentId, CancellationToken cancel)", false)]
        public async Task<IList<ContactAddress>> CreateAsync(IEnumerable<ContactAddress> entities, CancellationToken cancel, int parentId)
        {
            return await CreateAsync(entities, parentId, cancel).ConfigureAwait(false);
        }
        #pragma warning restore CA1068

        public async Task<IList<ContactAddress>> CreateAsync(IEnumerable<ContactAddress> entities, int parentId, CancellationToken cancel)
        {
            var request = _client.NewRequest("contacts/{contactId}/addresses", Method.POST);
            request.AddUrlSegment("contactId", parentId);
            request.AddJsonBody(entities);

            var results = await _client.ExecuteAsync<List<ContactAddress>>(request, cancel).ConfigureAwait(false);
            return results.Data;
        }

        public async Task<UpsertResult<ContactAddress>> UpsertAsync(ContactAddress entity,
            CancellationToken cancelToken, int? parentId = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var result = new UpsertResult<ContactAddress>();

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

        public List<ContactAddress> List(int contactId, int from, int size)
        {
            var request = _client.NewRequest("contacts/{contactId}/addresses");
            request.AddUrlSegment("contactId", contactId);
            request.AddQueryParameter("from", from.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("size", size.ToString(CultureInfo.InvariantCulture));

            var results = _client.Execute<List<ContactAddress>>(request);
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
            var request = _client.NewRequest("contacts/addresses/changes");
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
            var request = _client.NewRequest("contacts/{id}/metadata/{scope}");
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("scope", scope.ToString());

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken).ConfigureAwait(false);
            return result.Data;
        }

        public async Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data,
            CancellationToken cancellationToken, int? parentId = null)
        {
            var request = _client.NewRequest("contacts/{id}/metadata/{scope}", Method.PUT);
            request.AddUrlSegment("id", entityId.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("scope", scope.ToString());
            request.AddJsonBody(data);

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken).ConfigureAwait(false);
            return result.Data;
        }

        #endregion

    }
}
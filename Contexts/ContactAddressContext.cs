using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            var results = await _client.ExecuteAsync<ContactAddress>(request, cancelToken);
            return results.Data;
        }

        public async Task<ContactAddress> UpdateAsync(ContactAddress entity, CancellationToken cancel)
        {
            var request = _client.NewRequest("contacts/{id}/addresses/{addressId}", Method.PUT);
            request.AddUrlSegment("id", entity.ContactId.ToString());
            request.AddUrlSegment("addressId", entity.AddressID);
            request.AddBody(entity);

            var results = await _client.ExecuteAsync<ContactAddress>(request, cancel);
            return results.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancel, int? parentId = null)
        {
            if (!parentId.HasValue) throw new ArgumentException("Parent Id is required to delete contact addresss.");
            var request = _client.NewRequest("contacts/{id}/addresses/{AddressId}", Method.DELETE);
            request.AddUrlSegment("id", id);
            request.AddUrlSegment("AddressId", parentId.Value);

            await _client.ExecuteAsync<ContactAddress>(request, cancel);
        }


        public async Task<ContactAddress> CreateAsync(ContactAddress entity, CancellationToken cancel,
            int? parentId = null)
        {
            if (!parentId.HasValue) throw new ArgumentException("Parent id value is required to create contact address.");

            var result = await CreateAsync(new[] { entity }, cancel, parentId.Value);
            return result.FirstOrDefault();
        }

        public async Task<IList<ContactAddress>> CreateAsync(IEnumerable<ContactAddress> entities, CancellationToken cancel, int parentId)
        {
            var request = _client.NewRequest("contacts/{contactId}/addresses", Method.POST);
            request.AddUrlSegment("contactId", parentId);
            request.AddBody(entities);

            var results = await _client.ExecuteAsync<List<ContactAddress>>(request, cancel);
            return results.Data;
        }

        public async Task<UpsertResult<ContactAddress>> UpsertAsync(ContactAddress entity,
            CancellationToken cancelToken, int? parentId = null)
        {
            var result = new UpsertResult<ContactAddress>();

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

        public List<ContactAddress> List(int contactId, int from, int size)
        {
            var request = _client.NewRequest("contacts/{contactId}/addresses");
            request.AddUrlSegment("contactId", contactId);
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", size.ToString());

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
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel);
            return results.Data;
        }

        #endregion

        #region Subitems



        public async Task<TM> GetMetadataAync<TM>(MetadataScope scope, int id, CancellationToken cancellationToken,
            int? parentId = null)
        {
            var request = _client.NewRequest("contacts/{id}/metadata/{scope}");
            request.AddUrlSegment("id", id.ToString());
            request.AddUrlSegment("scope", scope.ToString());

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken);
            return result.Data;
        }

        public async Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data,
            CancellationToken cancellationToken, int? parentId = null)
        {
            var request = _client.NewRequest("contacts/{id}/metadata/{scope}", Method.PUT);
            request.AddUrlSegment("id", entityId.ToString());
            request.AddUrlSegment("scope", scope.ToString());
            request.AddBody(data);

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken);
            return result.Data;
        }

        #endregion

    }
}
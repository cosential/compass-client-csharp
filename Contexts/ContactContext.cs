using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Contexts;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client
{
    public class ContactContext: ICompassContext<Contact>
    {
        private readonly CompassClient _client;
        public ContactContext(CompassClient client)
        {
            _client = client;
        }

        public async Task<List<Contact>> CreateAsync(IEnumerable<Contact> entities, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("contacts", Method.POST);
            request.AddJsonBody(entities);
            var results = await _client.ExecuteAsync<List<Contact>>(request, cancelToken).ConfigureAwait(false);
            return results.Data;
        }

        public async Task<Contact> CreateAsync(Contact entity, CancellationToken cancelToken, int? parentId = null)
        {
            var entities = await CreateAsync(new[] {entity}, cancelToken).ConfigureAwait(false);
            return entities.FirstOrDefault();
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("contacts/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
            await _client.ExecuteAsync(request, cancelToken).ConfigureAwait(false);
        }

        #region CRUD

        public Contact Get(int id)
        {
            var request = _client.NewRequest("contacts/{id}");
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
            var results = _client.Execute<Contact>(request);

            return results.Data;

        }

        public async Task<Contact> GetAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            var request = _client.NewRequest("contacts/{id}");
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
            var results = await _client.ExecuteAsync<Contact>(request, cancelToken).ConfigureAwait(false);
            return results.Data;
        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            var request = _client.NewRequest("contacts/changes");
            request.AddQueryParameter("version", Convert.ToBase64String(rowVersion));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token).ConfigureAwait(false);
            return results.Data;
        }

        public IList<Contact> List(
                int from, int take, bool fullRecord = true
            )
        {
            var request = _client.NewRequest("contacts");

            request.AddQueryParameter("from", from.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("size", take.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("full", fullRecord.ToString());

            var results = _client.Execute<List<Contact>>(request);
            return results.Data;
        }

        public async Task<Contact> UpdateAsync(Contact entity, CancellationToken cancelToken)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (!entity.ContactId.HasValue) throw new Exception("entity id is required for update");

            var request = _client.NewRequest("contacts/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.ContactId.Value.ToString(CultureInfo.InvariantCulture));
            request.AddJsonBody(entity);

            // This looks like it was broken in Compass in this PR set https://bitbucket.org/cosential/compass/pull-requests/206#chg-Compass/Controllers/ContactsController.cs
            // I wasn't able to lock down exactly when, but the defect has been around for at least a year, so changing the interface now will break a bunch of external
            // users.  Therefore I'm changing the library to accomodate the defect (the contact is being sent in an array) since it is the new defacto API.
            var results = await _client.ExecuteAsync<IList<Contact>>(request, cancelToken).ConfigureAwait(false);
            return results.Data.FirstOrDefault();
        }

        public async Task<UpsertResult<Contact>> UpsertAsync(Contact entity, CancellationToken cancelToken,
            int? parentId = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var result = new UpsertResult<Contact>();

            if (entity.ContactId.HasValue)
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

        #endregion

        public List<ContactAddress> GetAddresses(int id)
        {
            var request = _client.NewRequest("contacts/{id}/addresses");
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("q", "DefaultInd:true");

            var result = _client.Execute<List<ContactAddress>>(request);
            return result.Data;
        }

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
    }
}

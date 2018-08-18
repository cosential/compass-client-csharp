using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            request.AddBody(entities);
            var results = await _client.ExecuteAsync<List<Contact>>(request, cancelToken);
            return results.Data;
        }

        public async Task<Contact> CreateAsync(Contact entity, CancellationToken cancelToken)
        {
            var entities = await CreateAsync(new[] {entity}, cancelToken);
            return entities.FirstOrDefault();
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("contacts/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());
            await _client.ExecuteAsync(request, cancelToken);
        }

        #region CRUD

        public Contact Get(int id)
        {
            var request = _client.NewRequest("contacts/{id}");
            request.AddUrlSegment("id", id.ToString());
            var results = _client.Execute<Contact>(request);

            return results.Data;

        }

        public async Task<Contact> GetAsync(int id, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("contacts/{id}");
            request.AddUrlSegment("id", id.ToString());
            var results = await _client.ExecuteAsync<Contact>(request, cancelToken);
            return results.Data;
        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            var request = _client.NewRequest("contacts/changes");
            request.AddQueryParameter("version", Convert.ToBase64String(rowVersion));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token);
            return results.Data;
        }

        public IList<Contact> List(
                int from, int take, bool fullRecord = true
            )
        {
            var request = _client.NewRequest("contacts");

            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("size", take.ToString());
            request.AddQueryParameter("full", fullRecord.ToString());

            var results = _client.Execute<List<Contact>>(request);
            return results.Data;
        }

        public async Task<Contact> UpdateAsync(Contact entity, CancellationToken cancelToken)
        {
            if (!entity.ContactId.HasValue) throw new Exception("entity id is required for update");

            var request = _client.NewRequest("contacts/{id}", Method.PUT);
            request.AddUrlSegment("id", entity.ContactId.Value.ToString());
            request.AddBody(entity);

            var results = await _client.ExecuteAsync<Contact>(request, cancelToken);
            return results.Data;
        }

        public async Task<UpsertResult<Contact>> UpsertAsync(Contact entity, CancellationToken cancelToken)
        {
            var result = new UpsertResult<Contact>();

            if (entity.ContactId.HasValue)
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

        #endregion

        public List<ContactAddress> GetAddresses(int id)
        {
            var request = _client.NewRequest("contacts/{id}/addresses");
            request.AddUrlSegment("id", id.ToString());
            request.AddQueryParameter("q", "DefaultInd:true");

            var result = _client.Execute<List<ContactAddress>>(request);
            return result.Data;
        }

        public async Task<TM> GetMetadataAync<TM>(MetadataScope scope, int id, CancellationToken cancellationToken)
        {
            var request = _client.NewRequest("contacts/{id}/metadata/{scope}");
            request.AddUrlSegment("id", id.ToString());
            request.AddUrlSegment("scope", scope.ToString());

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken);
            return result.Data;
        }

        public async Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data, CancellationToken cancellationToken)
        {
            var request = _client.NewRequest("contacts/{id}/metadata/{scope}", Method.PUT);
            request.AddUrlSegment("id", entityId.ToString());
            request.AddUrlSegment("scope", scope.ToString());
            request.AddBody(data);

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken);
            return result.Data;
        }
    }
}

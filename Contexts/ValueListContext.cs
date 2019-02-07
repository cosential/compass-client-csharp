using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Attributes;
using Cosential.Integrations.Compass.Client.Exceptions;
using Cosential.Integrations.Compass.Client.Models;
using Cosential.Integrations.Compass.Client.Models.Interfaces;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public class ValueListContext<T> : ICompassContext<T> where T : IValueList
    {
        private readonly CompassClient _client;
        private readonly CompassPathAttribute _config;
        private const int RefreshValue = 10;
        // ReSharper disable once StaticMemberInGenericType
        // In this case we do want each different type to have
        // and independant count.
        private static int _changeRequestCount;

        private static bool IsFullRefresh()
        {
            _changeRequestCount++;
            if (_changeRequestCount % RefreshValue != 0) return false;
            _changeRequestCount = 0;
            return true;
        }
        public ValueListContext(CompassClient client)
        {
            _client = client;
            _config = typeof(T).GetCompassPaths();
            if(_config == null)throw new ArgumentException($"ValueListContext the type:{typeof(T).Name} does not have Compass endpoints defined by a CompassEndpointAttribute.");
        }
        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            List<ChangeEvent> events;
            if (_config.HasEndpoint(EndpointType.Changes))
            {
                var request = _client.NewRequest(_config.Changes);
                request.AddQueryParameter("Version", Convert.ToBase64String(rowVersion));
                request.AddQueryParameter("includeDeleted", includeDeleted.ToString());

                var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, token);

                events = results.Data;
            } else if (_config.HasEndpoint(EndpointType.GetMany))
            {
                var lastNumberSeen = 0;
                try
                {
                    if(!IsFullRefresh())lastNumberSeen = BitConverter.ToInt32(rowVersion,0);
                }
                catch (Exception)
                {
                    // If we didn't get the last number of the set stored then
                    // we default to just processing the whole value list and on 
                    // the next pass the rowversion will have the correct value.
                }

                var rows = await GetAsync(includeDeleted, token);
                events = rows.Where(z=>((int)z.PrimaryKey) > lastNumberSeen)
                    .Select(
                        x => new ChangeEvent
                        {
                            Deleted = x.PrimaryIsDeleted,
                            Id = (int) x.PrimaryKey,
                            Version = BitConverter.GetBytes((int)x.PrimaryKey)
                        }
                    ).OrderBy(y=>y.Id).ToList();
            }
            else
            {
                throw new EndpointDoesNotSupportActionException($"The compass endpoint for {typeof(T).Name} does not support {EndpointType.Changes}.");
            }

            return events;
        }

        public async Task<T> GetAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            if (!_config.HasEndpoint(EndpointType.Get))
                throw new EndpointDoesNotSupportActionException($"The compass endpoint for {typeof(T).Name} does not support {EndpointType.Get}.");

            var request = _client.NewRequest(_config.Get);
            request.AddUrlSegment("id", id);
            var results = await  _client.ExecuteAsync<T>(request, cancelToken);

            return results.Data;
        }

        public async Task<List<T>> GetAsync(bool inlcudeDeleted ,CancellationToken cancelToken)
        {
            if (!_config.HasEndpoint(EndpointType.GetMany))
                throw new EndpointDoesNotSupportActionException($"The compass endpoint for {typeof(T).Name} does not support {EndpointType.GetMany}.");
            var aggregator = new List<T>();
            var from = 0;
            const int size = 50;
            while(true)
            {
                var request = _client.NewRequest(_config.GetMany);
                request.AddQueryParameter("includeDeleted", inlcudeDeleted.ToString());
                request.AddQueryParameter("from", from.ToString());
                request.AddQueryParameter("size", size.ToString());

                var results = await _client.ExecuteAsync<List<T>>(request, cancelToken);
                var sanityCheck = results.Data.First();

                // If the top result is already in our return set then the endpoint doesn't have
                // paging and we need to exit and move on.
                if (aggregator.Any(x => x.PrimaryKey.Equals(sanityCheck.PrimaryKey))) break;

                aggregator.AddRange(results.Data);
                if (results.Data == null || results.Data.Count < 50) break;
                from += size;
            }

            return aggregator;
        }

        public async Task<UpsertResult<T>> UpsertAsync(T entity, CancellationToken cancelToken, int? parentId = null)
        {
            var result = new UpsertResult<T>();
            try
            {
                if (entity.PrimaryKey != null && ((int)entity.PrimaryKey) != 0)
                {
                    result.Action = UpsertAction.Updated;
                    result.Data = await UpdateAsync(entity, cancelToken);
                }
                else
                {
                    result.Action = UpsertAction.Created;
                    result.Data = await CreateAsync(entity, cancelToken);
                }
            }
            catch (EndpointDoesNotSupportActionException e)
            {
                result.Action = UpsertAction.None;
                result.Message = e.Message;
            }

            return result;
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancelToken, int? parentId = null)
        {
            T result;
            if (_config.HasEndpoint(EndpointType.Create))
            {
                var request = _client.NewRequest(_config.Create, Method.POST);
                request.AddBody(entity);

                var response = await _client.ExecuteAsync<T>(request, cancelToken);

                result = response.Data;
            }
            else if (_config.HasEndpoint(EndpointType.CreateMany))
            {
                var list = new List<T> {entity};
                var request = await CreateAsync(list, cancelToken);
                result = request.First();
            }
            else
            {
                throw new EndpointDoesNotSupportActionException($"The compass endpoint for {typeof(T).Name} does not support {EndpointType.Create}.");
            }

            return result;
        }

        public async Task<List<T>> CreateAsync(IEnumerable<T> entities, CancellationToken cancelToken)
        {
            if (!_config.HasEndpoint(EndpointType.CreateMany))
                throw new EndpointDoesNotSupportActionException($"The compass endpoint for {typeof(T).Name} does not support {EndpointType.CreateMany}.");

            var request = _client.NewRequest(_config.CreateMany, Method.POST);
            request.AddBody(entities);

            var response = await _client.ExecuteAsync<List<T>>(request, cancelToken);

            return response.Data;
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken cancelToken)
        {
            if (!_config.HasEndpoint(EndpointType.Update))
                throw new EndpointDoesNotSupportActionException($"The compass endpoint for {typeof(T).Name} does not support {EndpointType.Update}.");

            var request = _client.NewRequest(_config.Update, Method.PUT);
            request.AddUrlSegment("id", entity.PrimaryKey);
            request.AddBody(entity);

            var response = await _client.ExecuteAsync<T>(request, cancelToken);

            return response.Data;
        }

        public async Task DeleteAsync(int id, CancellationToken cancelToken, int? parentId = null)
        {
            if (!_config.HasEndpoint(EndpointType.Delete))
                throw new Exception($"The compass endpoint for {typeof(T).Name} does not support {EndpointType.Delete}.");

            var request = _client.NewRequest(_config.Delete, Method.DELETE);
            request.AddUrlSegment("id", id);
            await _client.ExecuteAsync(request, cancelToken);
        }

        public Task<TM> GetMetadataAync<TM>(MetadataScope scope, int entityId, CancellationToken cancellationToken, int? parentId = null)
        {
            return Task.FromResult(default(TM));
        }

        public Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data, CancellationToken cancellationToken,
            int? parentId = null)
        {
            return Task.FromResult(default(TM));
        }
    }
}
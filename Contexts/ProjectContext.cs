using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public class ProjectContext : ICompassContext<Project>
    {
        private readonly CompassClient _client;
        public ProjectContext(CompassClient client)
        {
            _client = client;
        }

        public Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetAsync(int id, CancellationToken cancelToken)
        {
            throw new NotImplementedException();
        }

        public Task<UpsertResult<Project>> UpsertAsync(Project entity, CancellationToken cancelToken)
        {
            throw new NotImplementedException();
        }

        public Task<Project> CreateAsync(Project entity, CancellationToken cancelToken)
        {
            throw new NotImplementedException();
        }

        public Task<Project> UpdateAsync(Project entity, CancellationToken cancelToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancelToken)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetMetadataAync(MetadataScope scope, int entityId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> PutMetadataAsync(MetadataScope scope, int entityId, Dictionary<string, object> data, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }       
    }
}

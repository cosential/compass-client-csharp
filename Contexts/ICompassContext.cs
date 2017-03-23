using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public interface ICompassContext<T>
    {
        Task<List<ChangeEvent>> GetChangesAsync(byte[] rowVersion, bool includeDeleted, CancellationToken token);
        Task<T> GetAsync(int id, CancellationToken cancelToken);
        Task<UpsertResult<T>> UpsertAsync(T entity, CancellationToken cancelToken);
        Task<T> CreateAsync(T entity, CancellationToken cancelToken);
        Task<T> UpdateAsync(T entity, CancellationToken cancelToken);
        Task DeleteAsync(int id, CancellationToken cancelToken);
    }
}
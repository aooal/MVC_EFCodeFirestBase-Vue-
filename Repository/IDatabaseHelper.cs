using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC_EFCodeFirstWithVueBase.Repository
{
    public interface IDatabaseHelper
    {
        Task<IEnumerable<T>> GetAllAsync<T>(CancellationToken token) where T : class;
        Task<T?> GetByIdAsync<T>(string? id, CancellationToken token) where T : class;
        Task<T?> AddAsync<T>(T entity, CancellationToken token) where T : class;
        Task<T?> UpdateAsync<T>(T entity, CancellationToken token) where T : class;
        Task<T?> DeleteAsync<T>(string? id, CancellationToken token) where T : class;
    }
}

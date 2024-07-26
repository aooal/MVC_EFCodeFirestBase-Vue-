using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC_EFCodeFirstWithVueBase.Repository
{
    public interface IDatabaseHelper
    {
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
        Task<T?> GetByIdAsync<T>(string? id) where T : class;
        Task<T?> AddAsync<T>(T entity) where T : class;
        Task<T?> UpdateAsync<T>(T entity) where T : class;
        Task<T?> DeleteAsync<T>(string? id) where T : class;
    }
}

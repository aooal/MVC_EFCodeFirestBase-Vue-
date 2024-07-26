using Microsoft.EntityFrameworkCore;
using MVC_EFCodeFirstWithVueBase.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC_EFCodeFirstWithVueBase.Repository
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly AppDbContext _context;

        public DatabaseHelper(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync<T>(string? id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T?> AddAsync<T>(T entity) where T : class
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> UpdateAsync<T>(T entity) where T : class
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> DeleteAsync<T>(string? id) where T : class
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            return entity;
        }
    }
}

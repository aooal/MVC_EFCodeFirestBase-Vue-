using Microsoft.EntityFrameworkCore;
using MVC_EFCodeFirstWithVueBase.Models;

namespace MVC_EFCodeFirstWithVueBase.Services
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
    }
}

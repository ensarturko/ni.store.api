using Microsoft.EntityFrameworkCore;

namespace Ni.Store.Api.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
            
        }

        public DbSet<Entities.Store> Stores { get; set; }
    }
}

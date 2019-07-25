using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ni.Store.Api.Data.Repositories.Implementations
{
    public class StoreRepository : IStoreRepository
    {
        private readonly StoreDbContext _dbContext;

        public StoreRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Entities.Store> Get(int id)
        {
            return await _dbContext.Stores.FindAsync(id);
        }

        public IEnumerable<Entities.Store> Get()
        {
            return _dbContext.Stores.AsEnumerable();
        }

        public async Task<Entities.Store> Post(Entities.Store store)
        {
            var entry = await _dbContext.Stores.AddAsync(store);
            await _dbContext.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<Entities.Store> Update(Entities.Store store)
        {
            var entry = _dbContext.Stores.Update(store);
            await _dbContext.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<Entities.Store> GetFirst()
        {
            return await _dbContext.Stores.FirstOrDefaultAsync();
        }

        public async Task<bool> Head(string key, string value)
        {
            return await _dbContext.Stores.AnyAsync(x => x.Key.Equals(key) && x.Value.Equals(value));
        }

        public async Task Delete(Entities.Store store)
        {
            _dbContext.Stores.Remove(store);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete()
        {
            var items = _dbContext.Stores.AsEnumerable();
            _dbContext.RemoveRange(items);

            await _dbContext.SaveChangesAsync();
        }
    }
}
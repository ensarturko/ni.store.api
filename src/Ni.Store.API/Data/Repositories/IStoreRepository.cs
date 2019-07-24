using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ni.Store.Api.Data.Repositories
{
    public interface IStoreRepository
    {
        Task<Entities.Store> Get(int id);
        IEnumerable<Entities.Store> Get();
        Task<Entities.Store> Update(Entities.Store store);
        Task<bool> Head(string key, string value);
        Task Delete(Entities.Store store);
        Task Delete();
        
        //Task<Entities.Store> Put(int expireTime);

        // TODO: Wildcard get.
    }
}
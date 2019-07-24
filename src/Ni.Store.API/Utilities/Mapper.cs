using Ni.Store.Api.Models.Responses;

namespace Ni.Store.Api.Utilities
{
    public static class Mapper
    {
        public static StoreGetResponse ConvertToStoreGetResponse(this Data.Entities.Store store)
        {
            return new StoreGetResponse
            {
                Id = store.Id,
                Key = store.Key,
                Value = store.Value,
                ExpiresInMinutes = store.ExpiresInMinutes
            };
        }

        public static StorePutResponse ConvertToStorePutResponse(this Data.Entities.Store store)
        {
            return new StorePutResponse
            {
                Value = store.Value,
                Key = store.Key
            };
        }
    }
}

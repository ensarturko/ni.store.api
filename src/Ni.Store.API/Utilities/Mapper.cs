using Ni.Store.Api.Models.Responses;

namespace Ni.Store.Api.Utilities
{
    public static class Mapper
    {
        public static StoreGetResponse ConvertToStoreGetResponse(this Data.Entities.Store store)
        {
            return new StoreGetResponse
            {
                Key = store.Key,
                Value = store.Value,
                ExpirationTime = store.ExpirationTime,
                Id = store.Id
            };
        }

        public static StorePutResponse ConvertToStorePutResponse(this Data.Entities.Store store)
        {
            return new StorePutResponse
            {
                Id = store.Id,
                Value = store.Value,
                Key = store.Key,
                ExpirationTime = store.ExpirationTime
            };
        }

        public static StorePostResponse ConvertToStorePostResponse(this Data.Entities.Store store)
        {
            return new StorePostResponse
            {
                Id = store.Id,
                Value = store.Value,
                Key = store.Key,
                ExpirationTime = store.ExpirationTime
            };
        }
    }
}

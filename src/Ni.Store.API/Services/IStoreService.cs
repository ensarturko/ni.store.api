using Ni.Store.Api.Models.Requests;
using Ni.Store.Api.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ni.Store.Api.Services
{
    public interface IStoreService
    {
        Task<BaseResponse<StoreGetResponse>> Get(int id);
        BaseResponse<List<StoreGetResponse>> GetAll();
        Task<BaseResponse<StorePutResponse>> Put(int id, double? expireIn, StorePutRequest request);
        Task<BaseResponse<bool?>> Delete(int id);
        Task<BaseResponse<bool?>> Delete();
        Task Head(StoreHeadRequest request);
        Task<BaseResponse<StorePostResponse>> Post(StorePutRequest request);
        Task<Data.Entities.Store> GetFirst();
    }
}
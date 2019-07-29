using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ni.Store.Api.Models.Requests;
using Ni.Store.Api.Models.Responses;
using Microsoft.Extensions.Logging;
using Ni.Store.Api.Data.Repositories;
using Ni.Store.Api.Utilities;

namespace Ni.Store.Api.Services.Implementations
{
    public class StoreService : IStoreService
    {
        private readonly ILogger<StoreService> _logger;
        private readonly IStoreRepository _repository;

        public StoreService(ILogger<StoreService> logger, IStoreRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<BaseResponse<StoreGetResponse>> Get(int id)
        {
            var response = new BaseResponse<StoreGetResponse>();

            try
            {
                var store = await _repository.Get(id);

                if (store == null)
                {
                    response.Errors.Add("No such record exists.");
                }
                else if (store.ExpirationTime.HasValue && store.ExpirationTime < DateTime.Now)
                {
                    response.Errors.Add("The key is expired.");
                }
                else
                {
                    response.Data = store.ConvertToStoreGetResponse();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                response.Errors.Add("An error occurred while processing your request.");
            }

            return response;
        }

        public BaseResponse<List<StoreGetResponse>> GetAll()
        {
            var response = new BaseResponse<List<StoreGetResponse>>();

            try
            {
                var stores = _repository.Get().ToList();

                var storeList = new List<StoreGetResponse>();

                if (stores.Any())
                {
                    response.Data = new List<StoreGetResponse>();

                    foreach (var item in stores)
                    {
                        storeList.Add(item.ConvertToStoreGetResponse());
                    }

                    response.Data.AddRange(storeList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                response.Errors.Add("An error occurred while processing your request.");
            }

            return response;
        }

        public async Task<BaseResponse<StorePutResponse>> Put(int id, double? expireIn, StorePutRequest request)
        {
            var response = new BaseResponse<StorePutResponse>();

            if (!id.IsGreaterThanZero())
            {
                response.Errors.Add("Id must be greater than zero.");
            }

            if (string.IsNullOrEmpty(request.Key))
            {
                response.Errors.Add("Key cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(request.Value))
            {
                response.Errors.Add("Value cannot be null or empty.");
            }

            if (await _repository.Head(request.Key, request.Value))
            {
                response.Errors.Add("This record already exists.");
            }

            try
            {
                if (!response.HasError)
                {
                    var store = await _repository.Get(id);

                    if (store == null)
                    {
                        response.Errors.Add("No such record exists.");
                    }
                    else
                    {
                        store.Key = request.Key;
                        store.Value = request.Value;

                        if (expireIn.HasValue)
                        {
                            store.ExpirationTime = DateTime.Now.AddSeconds((double) expireIn);
                        }

                        var entity = await _repository.Update(store);
                        response.Data = entity.ConvertToStorePutResponse();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Errors.Add("An error occurred while processing your request.");
            }

            return response;
        }

        public async Task<BaseResponse<bool?>> Delete(int id)
        {
            var response = new BaseResponse<bool?>();

            if (!id.IsGreaterThanZero())
            {
                response.Errors.Add("Id must be greater than zero.");
            }

            try
            {
                if (!response.HasError)
                {
                    var store = await _repository.Get(id);
                    if (store != null)
                    {
                        await _repository.Delete(store);
                        response.Data = true;
                        _logger.LogInformation($"The record with id of {id} has been deleted.");
                    }
                    else
                    {
                        response.Data = null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Errors.Add("An error occurred while processing your request.");
            }

            return response;
        }

        public async Task<BaseResponse<bool?>> Delete()
        {
            var response = new BaseResponse<bool?>();

            try
            {
                await _repository.Delete();
                response.Data = true;
                _logger.LogInformation($"All of the records has been deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Data = false;
                response.Errors.Add("An error occurred while processing your request.");
            }

            return response;
        }

        public async Task Head(StoreHeadRequest request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                response.Data = await _repository.Head(request.Key, request.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                response.Errors.Add("An error occurred while processing your request.");
            }
        }

        public async Task<BaseResponse<StorePostResponse>> Post(StorePutRequest request)
        {
            var response = new BaseResponse<StorePostResponse>();

            if (string.IsNullOrEmpty(request.Key))
            {
                response.Errors.Add("Key cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(request.Value))
            {
                response.Errors.Add("Value cannot be null or empty.");
            }

            if (!request.ExpirationTime.HasValue)
            {
                request.ExpirationTime = DateTime.Now.AddDays(1);
            }

            var store = new Data.Entities.Store
            {
                Key = request.Key,
                Value = request.Value,
                ExpirationTime = request.ExpirationTime
            };

            if (await _repository.Head(store.Key, store.Value))
            {
                response.Errors.Add("This record already exists.");
            }

            if (!response.HasError)
            {
                var entity = await _repository.Post(store);
                response.Data = entity.ConvertToStorePostResponse();
            }

            return response;
        }

        public Task<Data.Entities.Store> GetFirst()
        {
            return _repository.GetFirst();
        }
    }
}
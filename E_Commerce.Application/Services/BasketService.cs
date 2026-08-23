using AutoMapper;
using E_Commerce.Application.Comman;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class BasketService : IBasketService
    {
        private readonly IBasketReposatory _basketReposatory;
        private readonly IMapper _mapper;

        public BasketService(IBasketReposatory basketReposatory,IMapper mapper)
        {
            _basketReposatory = basketReposatory;
            _mapper = mapper;
        }
        public async Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket, TimeSpan? TimeToLive = null, CancellationToken c = default)
        {
            //i will talk to reposatory and he want customer_Basket and i have BasketDto  
            //so we map from BasketDto to customer_Basket
            var customerBasket=_mapper.Map<CustomerBasket>(basket);
            var result= await _basketReposatory.CreateOrUpdateBasketAsync(customerBasket, TimeToLive, c);
            return result == null ?
                Result<BasketDto>.Fail(Error.Failure("BasketCreate.Failure", "Can Not Create Or Update Basket"))
                : Result<BasketDto>.Ok(basket);

        }

        public async Task<Result<bool>> DeleteBasketAsync(string BasketId, CancellationToken c = default)
        {
            var result = await _basketReposatory.DeleteBasketAsync(BasketId, c);
            return result ? Result<bool>.Ok(true) 
                : Result<bool>.Fail(Error.Failure("BasketDelete.Failure", "Can Not Delete Basket"));
        }

        public async Task<Result<BasketDto>> GetBasketAsync(string BasketId, CancellationToken c = default)
        {
            var customerBasket = await _basketReposatory.GetBasketAsync(BasketId, c);
            var basketDto = _mapper.Map<BasketDto>(customerBasket);
            return basketDto == null ?
                Result<BasketDto>.Fail(Error.NotFound("Basket Not Found"))
              : Result<BasketDto>.Ok(basketDto);
                           //or : basketDto;

        }
    }
}

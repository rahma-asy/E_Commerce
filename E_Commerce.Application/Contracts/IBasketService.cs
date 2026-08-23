using E_Commerce.Application.Comman;
using E_Commerce.Application.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IBasketService
    {
        //Get Basket=> Take Basketid and return Basketdto
        Task<Result<BasketDto>> GetBasketAsync(string BasketId, CancellationToken c=default);

        //create or update Basket=> Take Basket and return Basket after creation or update
        Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket,TimeSpan? TimeToLive=default,CancellationToken c=default);
        //delete Basket=> Take Basketid and return true or false

        //Result for controller to know he will return ok,problem details,not found
        Task<Result<bool>> DeleteBasketAsync(string BasketId, CancellationToken c = default);
    }
}

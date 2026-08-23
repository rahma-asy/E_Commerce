using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface IBasketReposatory
    {
        Task<CustomerBasket?> GetBasketAsync(string basketId,CancellationToken c=default);
        Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket,TimeSpan? timeToLive=default,CancellationToken c=default); 
        Task<bool> DeleteBasketAsync(string basketId, CancellationToken c = default);
    }
}

using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Baskets;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Reposatories
{
    internal class BasketReposatory : IBasketReposatory
    {
        //have database
        private readonly IDatabase _database;
        //full control i can add stream,store set,set key expire && have a connection
        public BasketReposatory(IConnectionMultiplexer connection) { _database = connection.GetDatabase();  }
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null, CancellationToken c = default)
        {
            //StringSetAsync take redis key and redis value are guid that front-end create[strings] so we translate it
            var value =JsonSerializer.Serialize(basket);
            //if it is exist update else create a key-value pair

           var result=await _database.StringSetAsync(basket.Id,value,timeToLive??TimeSpan.FromDays(7));
            return result? basket: null;
        }

        public async Task<bool> DeleteBasketAsync(string basketId, CancellationToken c = default)
        {
            var result=await _database.KeyDeleteAsync(basketId);
            return result;
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId, CancellationToken c = default)
        {
            var basket=await _database.StringGetAsync(basketId);//return string
            if(basket.IsNullOrEmpty) 
                return null;
            else
                //we want to return CustomerBasket
                return JsonSerializer.Deserialize<CustomerBasket>(basket!);
               //or
              //  return basket.IsNullOrEmpty?null: sonSerializer.Deserialize<CustomerBasket>(basket)
        }
    }
}

using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class CacheService : ICacheService
    {
        private readonly ICacheReposatory _cacheReposatory;

        public CacheService(ICacheReposatory cacheReposatory)
        {
           _cacheReposatory = cacheReposatory;
        }

        public async Task<string?> GetDataAsync(string cacheKey, CancellationToken c = default)
                                   =>await _cacheReposatory.GetAsync(cacheKey, c);

        public async Task SetDataAsync(string cacheKey, object cacheValue, TimeSpan? TimeToLive = null, CancellationToken c = default)
        {
            //it is a object and i want it as string
            var jsonValue = JsonSerializer.Serialize(cacheValue,new JsonSerializerOptions()
            {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase //كل مره كاشينج هيعمل بنفس الطريقه
            });
            await _cacheReposatory.SetAsync(cacheKey, jsonValue, TimeToLive, c); 
        }
    }
}

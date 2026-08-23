using E_Commerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Reposatories
{
    internal class CacheReposatory : ICacheReposatory
    {
        private readonly IDatabase _database;

        public CacheReposatory(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task SetAsync(string cacheKey, string cacheValue, TimeSpan? TimeToLive = null, CancellationToken c = default)
        {
            var value = await _database.StringSetAsync(cacheKey, cacheValue, TimeToLive ?? TimeSpan.FromDays(2));
        }

        public async Task<string?> GetAsync(string cacheKey, CancellationToken c = default)
        {
            var value = await _database.StringGetAsync(cacheKey);//will return it as redis value [have value or not]
            if(value.IsNullOrEmpty) return null;
            else
                return value.ToString();
        }
    }
}

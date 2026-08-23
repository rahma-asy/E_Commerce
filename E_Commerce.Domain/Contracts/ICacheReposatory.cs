using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace E_Commerce.Domain.Contracts
{
    public interface ICacheReposatory
    {
        //key is PATH
        Task<string?> GetAsync(string cacheKey,CancellationToken c=default);

        Task SetAsync(string cacheKey,string cacheValue,TimeSpan? TimeToLive=default , CancellationToken c=default);
    }
}

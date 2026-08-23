using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface ICacheService
          {
        //key is PATH
        Task<string?> GetDataAsync(string cacheKey, CancellationToken c = default);

        Task SetDataAsync(string cacheKey, object cacheValue, TimeSpan? TimeToLive = default, CancellationToken c = default);
    }
}

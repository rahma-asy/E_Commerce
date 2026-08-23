using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface IDataSeeder
    {
        Task SeedDataAsync(CancellationToken c=default);
    }
}

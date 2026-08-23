using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges(CancellationToken c = default);
        IGenaricReposatory<TEntity,Tkey> GetReposatory<TEntity,Tkey>() where TEntity : BaseEntity<Tkey>;
    }
}

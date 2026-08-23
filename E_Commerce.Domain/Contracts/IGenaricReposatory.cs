using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface IGenaricReposatory<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);

        Task<TEntity?> GetByIdAsync(TKey id,CancellationToken c=default);
        Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> Spec, CancellationToken c = default);

        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken c=default);
        //it need parts of query so he will send obj which implement iSpecification
        Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> Spec,CancellationToken c = default);
        Task<int> CountAsync(ISpecification<TEntity, TKey> Spec, CancellationToken c = default);//ISpecification only to apply craitaria


    }
}

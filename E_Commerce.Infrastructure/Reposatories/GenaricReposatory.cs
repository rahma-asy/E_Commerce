using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Commerce.Infrastructure.Reposatories
{
    internal class GenaricReposatory<TEntity, TKey> (StoreDbContext dbContext): IGenaricReposatory<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public void Add(TEntity entity)=>dbContext.Set<TEntity>().Add(entity);

        public void Remove(TEntity entity)=> dbContext.Set<TEntity>().Remove(entity);
        public void Update(TEntity entity)=> dbContext.Set<TEntity>().Update(entity);
     
        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken c = default)
                   => await dbContext.Set<TEntity>().FindAsync(id,c);
        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> spec, CancellationToken c = default)
         {
            var query = SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), spec);
            return await query.FirstOrDefaultAsync(c);

        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken c = default)
         => await dbContext.Set<TEntity>().ToListAsync(c);
        //هيبعتها جاهزه وانا انفذها
        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> Spec, CancellationToken c = default)
        {
            var query =  SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), Spec);

       return await query.ToListAsync(c);//هينفذها بقي
            
        }
        public async Task<int> CountAsync(ISpecification<TEntity, TKey> Spec, CancellationToken c = default)
        {
            return await SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(),Spec).CountAsync(c);
        }

    }
}

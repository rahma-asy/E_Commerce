using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Reposatories
{
    internal class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];

        public IGenaricReposatory<TEntity, Tkey> GetReposatory<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            var typeName = typeof(TEntity).Name;//get TEntity name to test if added before

            // If Exists -> Return
            if (_repositories.TryGetValue(typeName, out object? value))
                return (IGenaricReposatory<TEntity,Tkey>)value;
            else
            {
                // If Not -> Create - Store - Return
                var repo = new GenaricReposatory<TEntity, Tkey>(dbContext);
                _repositories[typeName] = repo;

                return repo;
            }
        }     



        public async Task<int> SaveChanges(CancellationToken c = default)=>await dbContext.SaveChangesAsync(c);
    }
}

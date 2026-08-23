using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface ISpecification<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        ICollection<Expression<Func<TEntity,object>>> IncludeExpressions { get; }
        Expression<Func<TEntity, bool>> Criteria { get; }
        Expression<Func<TEntity, object>>? Orderby { get; }
        Expression<Func<TEntity, object>>? OrderbyDesce { get; }
         int Take { get;}   
         int Skip { get;}
        bool IsPaginated { get;} //ولا لا هو دا اللي هيعرفني هعمل تقسيم ولا لاtake &skip مش كل شويه هشوفه باعت 

    }
}

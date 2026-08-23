using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{

    public abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
          //عشان تبقي اجباري مش براحته بدال ورثت مني لازم هتبقي عندك 
        public BaseSpecification(Expression<Func<TEntity, bool>> criteria)  { Criteria = criteria; }
        //cann't add direct
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        public Expression<Func<TEntity, object>>? Orderby { get; private set; }//if i have obj of ISpecification i cannot edit it

        public Expression<Func<TEntity, object>>? OrderbyDesce { get; private set; }

        public int Take{ get; private set; }
                             
        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }

        //he should ask BaseSpecification if he can add in IncludeExpressions  >if i cAN I will do set;
        //helper methos > if i inherit BaseSpecification i will inherit it
        //send expretion and i will add it in IncludeExpressions
        protected void AddInclude(Expression<Func<TEntity, object>> include)
        {
            IncludeExpressions.Add(include);  //besause IncludeExpressions is ICollection method
        }
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            Orderby= orderByExpression;  
        }
        protected void AddOrderyByDesce(Expression<Func<TEntity, object>> orderByDesceExpression)
        {
      OrderbyDesce=  orderByDesceExpression;  
        }

        protected void ApplyPagination(int pageSize,int pageIndex) // واحسب انت بقي 
        {
     //not opptional ,if we want is opptional take it from user as a parameter and ckeck if it is true do pagination else no pagination

            IsPaginated = true;
            Take= pageSize;
            Skip = (pageIndex-1)*pageSize;

        }

    }
}

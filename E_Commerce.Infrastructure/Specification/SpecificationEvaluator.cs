using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Specification
{
    //he send all components in obj and i check if it is exist or not[add or skip]

    internal static class SpecificationEvaluator
    {
        //take Specifications and return Query              //entry point >return it only when there is no Specifications
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> spec) where TEntity : BaseEntity<TKey>
        {
            //entry point
            var query = inputQuery; // dbContext.Set<TEntity>();
                                    //components
            if (spec.Criteria != null)
            {
                query=query.Where(spec.Criteria);
            }

            if (spec.IncludeExpressions.Any())
                {
                //foreach (var expression in spec.IncludeExpressions)
                //{
                //    query = query.Include(expression);
                //}   or
                //i have start and loop on collection ,I do the same thing every time[take Expressions from collection and put it in iclude and I add to that query,repeat ]
                query = spec.IncludeExpressions
                             .Aggregate(query, (current,nextExp)=>current.Include(nextExp)); //without inputQuery he will reasigh [delete the old and add what's returns from Aggregate]
             
            }

            if (spec.Orderby != null) 
            {
                query=query.OrderBy(spec.Orderby);
            }

            else if(spec.OrderbyDesce != null)
            {
                query = query.OrderByDescending(spec.OrderbyDesce);
            }

            if (spec.IsPaginated == true)
            {
            query=query.Skip(spec.Skip).Take(spec.Take);
                    }
            return query;
        }
        
    }
}

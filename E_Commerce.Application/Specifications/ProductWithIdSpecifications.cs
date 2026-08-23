using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class ProductWithIdSpecifications:BaseSpecification<Product, int>
    {
        public ProductWithIdSpecifications(HashSet<int> ProductIds) :base(P=>ProductIds.Contains(P.Id))
        { }
    }
}

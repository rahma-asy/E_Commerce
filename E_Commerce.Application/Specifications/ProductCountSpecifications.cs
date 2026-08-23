using E_Commerce.Application.Comman;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class ProductCountSpecifications:BaseSpecification<Product, int> 
    {
        public ProductCountSpecifications(ProductQueryParams queryParams):
            //composition of critaria
            base(P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId.Value)
           && (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId.Value)
           && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {
//[pagination or ....]ليها هنا هو مش هينفذها apply لو بعت اي حاجه تانيه بدال معملتش  
        }
    }
}

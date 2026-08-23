using E_Commerce.Application.Comman;
using E_Commerce.Application.Services;
using E_Commerce.Domain.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    //علي حسب بقي كل واحد عايز يبقي عنده ايه بالظبط بس كدا كدا الاساس واحد
    internal class ProductWithTypeAndBrandSpecification: BaseSpecification<Product, int>
    {
        //when he create obj of ProductWithTypeAndBrandSpecification he add productbrand and producttype
        //used with get all products
    
        public ProductWithTypeAndBrandSpecification(ProductQueryParams queryParams) :base(
           //Have Many Cases
           //1-BrandId is Not NULL >P=>P.BrandId=BrandId
           //2-TypeId is Not NULL >P=>P.TypeId=TypeId
           //3-BrandId And TypeId are Not NULL >P=>P.BrandId=BrandId&&P=>P.TypeId=TypeId
           //if i send 3 direct and i mean don't filter with TypeId he anderstand null as a value so there is no data[TypeId=null &&BrandId=x]                                 
           //  P => (BrandId==null ||P.BrandId == BrandId) &&(TypeId == null || P.TypeId == TypeId)
           //or
           //if !he have don't value will move to P.BrandId == BrandId),if !BrandId.HasValue is true > P => (true) [مع اول ترو هو كله ترو]
           P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId.Value)
           && (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId.Value)
           &&(string.IsNullOrWhiteSpace(queryParams.SearchValue)||P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))

           //true & true>all without any felteration on BrandId or TypeId )
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

            switch(queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:AddOrderBy(P => P.Name); break;
                case ProductSortingOptions.PriceAsc: AddOrderBy(P => P.Price); break;
                case ProductSortingOptions.NameDesc: AddOrderyByDesce(P => P.Name); break;
                case ProductSortingOptions.PriceDesc: AddOrderyByDesce(P => P.Price); break;
                default:AddOrderBy(P => P.Id); break; 

            }

            ApplyPagination(queryParams.PageSize,queryParams.PageIndex);
        }
        //used with get product by id
        public ProductWithTypeAndBrandSpecification(int id) : base(x => x.Id == id) //no where , you send id and i send expretion to chaining
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
    }
}

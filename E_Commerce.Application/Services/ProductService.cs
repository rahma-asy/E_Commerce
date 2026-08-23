using AutoMapper;
using E_Commerce.Application.Comman;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken c = default)
        { 
           var brands= await _unitOfWork.GetReposatory<ProductBrand,int>().GetAllAsync(c);
            //mapping
            var data= _mapper.Map<IReadOnlyList<BrandDto>>(brands);
            return Result<IReadOnlyList<BrandDto>>.Ok(data);
        }
       
        public async Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken c = default)
        {
            var types = await _unitOfWork.GetReposatory<ProductType, int>().GetAllAsync(c);
            //mapping
            var data = _mapper.Map<IReadOnlyList<TypeDto>>(types);
            return Result<IReadOnlyList<TypeDto>>.Ok(data);

        }
       
        public async Task<Result<PaginatedResult<ProductDto>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken c = default)
        {
            var spec = new ProductWithTypeAndBrandSpecification(queryParams);
            var countSpec = new ProductCountSpecifications(queryParams);//only set critaria
            var products = await _unitOfWork.GetReposatory<Product, int>().GetAllAsync(spec);
            //mapping
            var data = _mapper.Map<IReadOnlyList<ProductDto>>(products);
            // pagination هيبقي فيها العدد بعد ال productsعشان ال 
            //انا عايزا عدد العناصر كلها اللي بتحقق الشرط لو موجود اصلا 
            var countOfAllProducts = await _unitOfWork.GetReposatory<Product, int>().CountAsync(countSpec);
            var result = new PaginatedResult<ProductDto>(queryParams.PageIndex,queryParams.PageSize, countOfAllProducts, data);
            return Result<PaginatedResult<ProductDto>>.Ok(result);

        }

        public async Task<Result<ProductDto>> GetProductByIdAsync(int id, CancellationToken c = default)
        {
            var spec = new ProductWithTypeAndBrandSpecification(id);

            var product = await _unitOfWork.GetReposatory<Product, int>().GetByIdAsync(spec,c);
            //mapping
            if (product == null)
              // return Result<ProductDto>.Fail(Error.NotFound("Product Not Found", $"Product With id {id} Is Not Found"));
            return Error.NotFound("Product Not Found", $"Product With id {id} Is Not Found");//error mean ok

            //var data = _mapper.Map<ProductDto>(product); 
            //return Result<ProductDto>.Ok(data);
            return _mapper.Map<ProductDto>(product);//value mean ok

        }
    }
}

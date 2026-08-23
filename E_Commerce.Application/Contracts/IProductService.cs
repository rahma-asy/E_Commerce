using E_Commerce.Application.Comman;
using E_Commerce.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IProductService
    {
        Task<Result<PaginatedResult<ProductDto>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken c=default);
        Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken c = default);

        Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken c = default);

        Task<Result<ProductDto>> GetProductByIdAsync(int id,CancellationToken c=default);

    }
}

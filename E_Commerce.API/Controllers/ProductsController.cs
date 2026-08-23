using E_Commerce.API.Attributes;
using E_Commerce.Application.Comman;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace E_Commerce.API.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #region get all Products 
        [HttpGet]
        [RedisCashe(90)]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams queryParams,CancellationToken c)
        {
            var result = await _productService.GetAllProductsAsync(queryParams, c);
            // return Ok(result); //create obj of ok_result and status code of response is 200 ok
            return ToActionResult(result); //بعتها وهي هتتصرف فيها بقي علي حسب اللي راجع وكل دا
            //if ToActionResult must be genaric because it return data he will know from(result)
        }
        #endregion

        #region get all types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetAllTypes(CancellationToken c)
        {
            var result = await _productService.GetAllTypesAsync(c);
            return ToActionResult(result);
        }
        #endregion

        #region get all brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetAllBrands(CancellationToken c)
        {
            var result = await _productService.GetAllBrandsAsync(c);
            return ToActionResult(result);
        }
        #endregion

        #region get product by id
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]//defult
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)] //have bad senario so added in swagger

        public async Task<ActionResult<ProductDto>> GetProductById(int id, CancellationToken c)
        {
            var result = await _productService.GetProductByIdAsync(id, c);
            return ToActionResult(result);
        }
        #endregion
    }

}

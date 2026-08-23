using E_Commerce.Application.Comman;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.DTOs.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{

    public class BasketsController : ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketsController(IBasketService basketService) { _basketService = basketService; }

        //GET     BaseUrl/api/Baskets/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]//200okعشان ممكن ترجع حاجه غير 
        [ProducesResponseType(typeof(BasketDto),StatusCodes.Status200OK)]

        public async Task<ActionResult<BasketDto>> GetBasket(string id, CancellationToken c)
        {
            var result = await _basketService.GetBasketAsync(id, c);
            return ToActionResult(result);

        }



        //POST    BaseUrl/api/Baskets  -> body[BasketDto]
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket, CancellationToken c)
        {
            var result = await _basketService.CreateOrUpdateBasketAsync(basket,c:c);
            return ToActionResult(result);
        }
        //DELETE     BaseUrl/api/Baskets/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket( string id, CancellationToken c)
        {
            var result = await _basketService.DeleteBasketAsync(id, c);
            return ToActionResult(result);
        }
    }
}

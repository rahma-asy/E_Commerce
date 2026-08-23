using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Domain.Entities.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{

    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // POST /api/Orders/ Create Order => Order (BasketId - DeliveryMethod Id , Ship To Address) [Email]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto, CancellationToken c)
        => ToActionResult(await _orderService.CreateOrderAsync(orderDto, GetEmailFromToken(), c));

      
        // GET /api/Orders/Orders Of User => [Email] -> User Orders For Logged In User

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders(CancellationToken c)
               => ToActionResult(await _orderService.GetAllOrdersForUserAsync(GetEmailFromToken(), c));


        // GET /api/Orders/{id}  Order Of User => Id + [Email] -> User Order For Logged In User
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrdeById(Guid id, CancellationToken c)
                => ToActionResult(await _orderService.GetOrderByIdAndEmailAsync(id, GetEmailFromToken(), c));

        // GET /api/Orders/Delivery Method => List Of Delivery Method
        [AllowAnonymous]
        [HttpGet("DeliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethods( CancellationToken c)
                => ToActionResult(await _orderService.GetDeliveryMethodsAsync(c));

    }
}

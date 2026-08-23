using E_Commerce.Application.Comman;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto,string email,CancellationToken c);
        Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForUserAsync(string email, CancellationToken c);
        Task <Result<OrderToReturnDto>> GetOrderByIdAndEmailAsync(Guid Id,string email, CancellationToken c);
        Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodsAsync( CancellationToken c);
    }
}

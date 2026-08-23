
using AutoMapper;
using E_Commerce.Application.Comman;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IBasketReposatory _basketReposatory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IBasketReposatory basketReposatory,IUnitOfWork unitOfWork,IMapper mapper)
        {
            _basketReposatory = basketReposatory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken c)
        {
            var basket = await _basketReposatory.GetBasketAsync(orderDto.BasketId, c);

            if (basket == null)
                return Error.NotFound("Basket Not Found", $"Basket With id {orderDto.BasketId} Is Not Found");

            if (basket.Items.Count == 0)
                return Error.Validation("Basket Is Empty", $"Can Not Create Order With Basket id {basket.Id}");

            var exOrder = await _unitOfWork.GetReposatory<Order, Guid>().GetByIdAsync(new PaymentIntentSpec(basket.PaymentIntentId),c);
             if(exOrder!=null)  _unitOfWork.GetReposatory<Order, Guid>().Remove(exOrder);


            // Items (Order Item)
            var orderItems = new List<OrderItem>(basket.Items.Count);

            var productIds = basket.Items.Select(x => x.Id).ToHashSet();

            var products = (await _unitOfWork.GetReposatory<Product, int>()
                    .GetAllAsync(new ProductWithIdSpecifications(productIds), c))
                    .ToDictionary(x => x.Id);

            foreach (var item in basket.Items)
            {
                // Get Productec
                if (!products.TryGetValue(item.Id, out Product product))
                    return Error.NotFound("Product Not Found",$"Product With Id {item.Id} Is Not Found ");

                orderItems.Add(new OrderItem()
                {
                    Price = product.Price,
                    Quantity = item.Quantity,
                    ProductItemOrdered = new ProductItemOrdered()
                    {
                        PictureUrl = product.PictureUrl,
                        ProductId = product.Id,
                        ProductName = product.Name
                    }
                });
            }

            // ShipToAddress (Order Address)
            var orderAddress = _mapper.Map<OrderAddress>(orderDto.ShipToAddress);

            // Delivery Method
            var deliveryMethod = await _unitOfWork.GetReposatory<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId);

            if (deliveryMethod == null)
                return Error.NotFound("Delivery Method Not Found",$"Delivery Method With Id {orderDto.DeliveryMethodId} Is Not Found");

            // Sub Total
            var subTotal = orderItems.Sum(x => x.Quantity * x.Price);

            // Create Order
            var order = new Order(email,orderAddress,orderItems,deliveryMethod,subTotal,basket.PaymentIntentId);

            _unitOfWork.GetReposatory<Order, Guid>().Add(order); // Local

            var result = await _unitOfWork.SaveChanges(c);

            if (result == 0)
                return Error.Failure("Order Save Failed","Can Not Create order");
         
            else
            {
                await _basketReposatory.DeleteBasketAsync(orderDto.BasketId, c);

                return _mapper.Map<OrderToReturnDto>(order);
            }
        }

        public async Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForUserAsync(string email, CancellationToken c)
        {
            var orders=await _unitOfWork.GetReposatory<Order,Guid>().GetAllAsync(new OrderSpecifications(email),c);
           if(orders.Any())
            {
                var result = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
                return Result<IReadOnlyList<OrderToReturnDto>>.Ok(result);
            }
           else
             return Error.NotFound("Orders Not Found", $"No Orders Found For User");
        }

        public async Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailAsync(Guid Id, string email, CancellationToken c)
        {
            var order = await _unitOfWork.GetReposatory<Order, Guid>().GetByIdAsync(new OrderSpecifications(Id,email), c);
            if (order!=null)
            {
                //var result = _mapper.Map<OrderToReturnDto>(order);
                //return Result<OrderToReturnDto>.Ok(result);
                //or
                return _mapper.Map<OrderToReturnDto>(order);


            }
            else
                return Error.NotFound("Order Is Not Found", "No Order Found With This Id");
        }
      
        public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodsAsync(CancellationToken c)
        {
            var deliveryMethods = await _unitOfWork.GetReposatory<DeliveryMethod,int>().GetAllAsync(c);
            if (deliveryMethods != null)
            {
                return Result<IReadOnlyList<DeliveryMethodDto>>.Ok(_mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryMethods));


            }
            else
                return Error.NotFound("Delivery Methods Are Not Found", "There Is No Delivery Methods");
        }
    }
}

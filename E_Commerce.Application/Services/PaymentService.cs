using AutoMapper;
using E_Commerce.Application.Comman;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketReposatory _basketReposatory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IMapper _mapper;
        private readonly PaymentGatewaySettings _paymentGatewaySettings;

        public PaymentService(IBasketReposatory basketReposatory,
            IUnitOfWork unitOfWork,
            IPaymentGateway paymentGateway,
            IOptions<PaymentGatewaySettings> options,IMapper mapper
            )
        {
            _basketReposatory = basketReposatory;
            _unitOfWork = unitOfWork;
            _paymentGateway = paymentGateway;
            _mapper = mapper;
            _paymentGatewaySettings = options.Value;
        }
        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken c)
        {
            #region 1. Get Basket [Validate]

            var basket = await _basketReposatory.GetBasketAsync(basketId, c);
            if (basket == null)
                return Error.NotFound("Basket Is Not Found", $"Basket With Id {basketId} Is Not Found");

            if (basket.Items.Count == 0)
                return Error.Validation("Basket Is Empty");
            #endregion

            #region 2. Get Delivery Method Cost
            if (!basket.DeliveryMethodId.HasValue)
                return Error.Validation("Delivery Method Id Is Required");
            var deliveryMethod = await _unitOfWork.GetReposatory<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value, c);
            if (deliveryMethod == null)
                return Error.NotFound("Delivery Method Is Not Found");

            basket.ShippingPrice = deliveryMethod.Price;
            #endregion

            #region 3. Product Prices
            var productsIds = basket.Items.Select(x => x.Id).ToHashSet();

            var products = (await _unitOfWork.GetReposatory<Product, int>().GetAllAsync(new ProductWithIdSpecifications(productsIds), c)).ToDictionary(x => x.Id);

            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Error.NotFound("Product Not Found");

                item.Price = product.Price;
            }
            #endregion

            #region 4. Total Amount
            var subTotal = basket.Items.Sum(i => i.Price * i.Quantity);
            var amount = (long)((subTotal + deliveryMethod.Price) * 100);
            #endregion

            // 5.1 PaymentIntentId Empty => Create - Put PaymentIntentId + ClientSecret In Basket
            
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create
                var result = await _paymentGateway.CreatePaymentIntentAsync(amount,_paymentGatewaySettings.DefultCurrency,c);

                basket.PaymentIntentId = result.PaymentIntentId;
                basket.ClientSecret = result.ClientSecret;
            }
            else
            {
                // 5.2 PaymentIntentId Not Empty -> Update PaymentIntent
                // Update
                await _paymentGateway.UpdatePaymentIntentAsync(amount,basket.PaymentIntentId,c);
                //  var intent = await _paymentIntentService.UpdateAsync(paymentIntentId, options, cancellationToken: c);
                //        public string? PaymentIntentId { get; set; }

            }

            await _basketReposatory.CreateOrUpdateBasketAsync(basket, c:c);

            // Return BasketDto Updated
            // Return BasketD to Updated
            return _mapper.Map<BasketDto>(basket);
        }

        public async Task PaymentFailed(string paymentIntentId)
        {
            var order = await _unitOfWork.GetReposatory<Order, Guid>()
                .GetByIdAsync(new PaymentIntentSpec(paymentIntentId));

            if (order == null)
                return;

            order.Status = OrderStatus.PaymentFailed;

            await _unitOfWork.SaveChanges();
        }

        public async Task PaymentSucceeded(string paymentIntentId)
        {
            var order = await _unitOfWork.GetReposatory<Order, Guid>()
                .GetByIdAsync(new PaymentIntentSpec(paymentIntentId));

            if (order == null)
                return;

            order.Status = OrderStatus.PaymentReceived;

            await _unitOfWork.SaveChanges();
        }
    }
}

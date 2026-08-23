using E_Commerce.Application.Comman;
using E_Commerce.Application.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IPaymentService
    {
        Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken c=default);
        Task PaymentSucceeded(string paymentIntentId);

        Task PaymentFailed(string paymentIntentId);

    }
}

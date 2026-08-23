using E_Commerce.Application.Comman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IPaymentGateway  //contract that his implementation will deals with Stripe
    {
            // Create PaymentIntent
            // amount + Currency => PaymentIntentId + ClientSecret

            Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount,string currency,CancellationToken c=default);

            // Update PaymentIntent
            // PaymentIntentId + Amount => PaymentIntent + ClientSecret

            Task<PaymentIntentResult> UpdatePaymentIntentAsync(decimal amount,string paymentIntentId,CancellationToken c=default);
        }
    }


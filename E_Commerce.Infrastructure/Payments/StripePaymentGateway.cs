using E_Commerce.Application.Comman;
using E_Commerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Payments
{
    internal class StripePaymentGateway : IPaymentGateway

    {
        private readonly PaymentIntentService _paymentIntentService = new();

        public StripePaymentGateway(IOptions<PaymentGatewaySettings> options)
        {
            StripeConfiguration.ApiKey = options.Value.SecretKey;
        }
        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken c)
        {
           var options=new PaymentIntentCreateOptions() {
               Currency = currency.ToLower(),
               Amount=(long)amount ,
               PaymentMethodTypes = ["card"]
           };
           var intent=await _paymentIntentService.CreateAsync(options,cancellationToken:c);
            return new PaymentIntentResult(intent.Id,intent.ClientSecret);
        }

        public async Task<PaymentIntentResult> UpdatePaymentIntentAsync(decimal amount, string paymentIntentId, CancellationToken c)
        {
            var options = new PaymentIntentUpdateOptions()
            {
                    Amount = (long)amount,
            };
            var intent = await _paymentIntentService.UpdateAsync(paymentIntentId,options, cancellationToken: c);
            return new PaymentIntentResult(intent.Id, intent.ClientSecret);
        }
    }
}

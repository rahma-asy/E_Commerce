using E_Commerce.Application.Comman;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce.API.Controllers
{

    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly PaymentGatewaySettings _paymentGatewaySettings;

        //create or update paymentintent
        public PaymentsController(IPaymentService paymentService,IOptions<PaymentGatewaySettings> options)
        {
            _paymentService = paymentService;
            _paymentGatewaySettings = options.Value;
        }
        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePatmentIntent(string basketId, CancellationToken c)
        => ToActionResult(await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId, c));
        //POST baseUrl/api/payments/basketid
        //baskedid => basketDto updated with ClientSecret & PaymentIntentId
        #region webhook [WE must have a Frontend Project to sent requst to Stripe & Stripe call me via StripeWebhook endpoint ]
        //   [HttpPost("webhook")]
        //public async Task<IActionResult> StripeWebhook()
        //{
        //    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(); //request اول معلومه عشان يبقي معايا كل معلومات ال

        //    try
        //    {
        //        var stripeEvent = EventUtility.ConstructEvent( //helrer class
        //            json,
        //            Request.Headers["Stripe-Signature"], //stripe ف اقراها عشان تتاكد انها من headerلازم اتاكد انها فال
        //            _paymentGatewaySettings.WebhookSecret); //WebhookSecret that stripe will sent to know the event[same  WebhookSecret= same event]
        //        //we see templet and write my logic [implement my customb logic]
        //        switch (stripeEvent.Type) //get type[Succeeded or not]
        //        {
        //            case EventTypes.PaymentIntentSucceeded:
        //                //get data[PaymentIntent]
        //                var succeededPaymentIntent = stripeEvent.Data.Object as PaymentIntent;//change staus for order  with this PaymentIntent ID
        //                if (succeededPaymentIntent is not null)
        //                    await _paymentService.PaymentSucceeded(succeededPaymentIntent.Id);

        //                break;

        //            case EventTypes.PaymentIntentPaymentFailed:

        //                var failedPaymentIntent = stripeEvent.Data.Object as PaymentIntent;

        //                if (failedPaymentIntent is not null)
        //                    await _paymentService.PaymentFailed(failedPaymentIntent.Id);

        //                break;

        //            default:
        //                break;
        //        }

        //        return Ok();//my response
        //    }
        //    catch (StripeException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return BadRequest(ex.Message);
        //    }
        // } 
        #endregion
    }
}

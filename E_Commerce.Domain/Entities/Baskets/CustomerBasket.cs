using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Baskets
{
    public class CustomerBasket //model that deals with redis
    {
        public string Id { get; set; } = default!;//string because it will created as a guid by Frontand [When he send basket he will send id]
        public ICollection<BasketItem> Items { get; set; } = [];
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        //data will take it from front and i will sent it
        public decimal? ShippingPrice { get; set; }
        public int? DeliveryMethodId { get; set; }
        //when clint check out and need to talk with strip he will call me frist to create payment entit for this in stripe
        
    }
}

using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class PaymentIntentSpec : BaseSpecification<Order, Guid>
    {
        public PaymentIntentSpec(string paymentIntentId) : base(P =>P.PaymentIntentId==paymentIntentId)
        { }
    }
}

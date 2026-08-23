using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class OrderSpecifications : BaseSpecification<Order, Guid>
    {
        public OrderSpecifications(string email) : base(x=>x.BuyerEmail==email)
        {
            AddInclude(x => x.DeliveryMethod);//to load them
            AddInclude(x => x.Items);
            AddOrderyByDesce(x => x.OrderDate);
        }
        public OrderSpecifications(Guid id,string email) : base(x => x.BuyerEmail == email&& x.Id==id)
        {
            AddInclude(x => x.DeliveryMethod);//to load them
            AddInclude(x => x.Items);
         
        }
    }
}

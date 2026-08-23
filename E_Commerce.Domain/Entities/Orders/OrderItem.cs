using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Orders
{
    public class OrderItem:BaseEntity<int>
    {
       public ProductItemOrdered ProductItemOrdered { get; set; }//i need a snapshot to my order no need for your changes in Product[main entity]
        public decimal Price { get; set; }//can changed [added discount]
        public int Quantity { get; set; }
        }
    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Orders
{
    //we seperate it to application value object
    public class ProductItemOrdered
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = default!;

        public string PictureUrl { get; set; } = default!;
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Baskets
{
    public class BasketItem //not a table in database so no inheritance from BaseEntity so no configuration and dbsets
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public int  Quantity { get; set; }
    }
}

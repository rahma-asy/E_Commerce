using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Products
{
    public class ProductBrand:BaseEntity<int>
    {
        public string Name { get; set; } = default!;

    }
}

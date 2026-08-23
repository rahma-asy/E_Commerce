using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Products
{
    public class ProductDto //to avoid direct use for product
    {
        public int Id { get; set; } 
        public string Name { get; set; }=default!;
        public string Description { get; set; }=default!;
        public string PictureUrl { get; set; } = default!;
        public string ProductBrand { get; set; }=default!; 
        public string ProductType { get; set; }=default!;

        public decimal Price { get; set; }

    }
}

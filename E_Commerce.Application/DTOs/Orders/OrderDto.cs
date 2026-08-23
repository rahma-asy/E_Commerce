using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Orders
{
    public class OrderDto//will take it
    {
        [Required]
        public string BasketId { get; set; } = default!;
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public AddressDto ShipToAddress { get; set; } = default!;
    }
}

using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Orders
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; }
        public AddressDto ShipToAddress { get; set; } = default!;
        public string DeliveryMethod { get; set; } = default!;
        public string Status { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }

        public decimal SubTotal { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public decimal Total { get; set; }      
    }
}

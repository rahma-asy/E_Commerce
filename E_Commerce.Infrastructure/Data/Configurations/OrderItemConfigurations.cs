using E_Commerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.Price).HasColumnType("decimal(8,2)");
            builder.OwnsOne(o => o.ProductItemOrdered, product =>
            {
                product.Property(x => x.ProductName).HasMaxLength(100);
                product.Property(x => x.PictureUrl).HasMaxLength(200);
            });
        }
    }
}

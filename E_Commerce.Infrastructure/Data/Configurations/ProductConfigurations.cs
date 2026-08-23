using E_Commerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(x => x.ProductBrand).WithMany().HasForeignKey(X => X.BrandId);
            builder.HasOne(x => x.ProductType).WithMany().HasForeignKey(X => X.TypeId);

           builder.Property(x => x.Price). HasColumnType("decimal(18,2)");
            builder.Property(x => x.Name).HasMaxLength(100);
           builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.PictureUrl).HasMaxLength(200);
        }
    }
}

using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data
{
    //anytime obj created he will send DbContextOptions
    //internal to:no one have direct access on  StoreDbContext
    internal class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
    {
        public DbSet<Product>  Products { get; set; }
        public DbSet <ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //get Assembly where code is currently being execute in [code in infractructure.StoreDbContext  so get from E_Commerce.Infrastructure.dll]  
            //if configuration in another project
            //لو عملت الميثود دي فمكان تاني كدا هيرجع الاعداات اللي فيها اللي هي اصلا مش موجوده
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//in compilation time
                                         
            
            //will filter
           // modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly);//[StoreDbContext] حتي لو اتغير مكان الاستدعاء كدا كدا هيجيب من الطبقه اللي فيها الكلاس الاساسي []  

        }
    }
}

using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding
{
    internal class CatalogDataSeeder(StoreDbContext dbContext,ILogger<CatalogDataSeeder> logger) : IDataSeeder
    {
        

        public async Task SeedDataAsync(CancellationToken c=default)
        {
            try {
                var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(c);//return collection of pending migrations's name
                if (PendingMigrations.Any()) await dbContext.Database.MigrateAsync(c);

                //seeding
                // F:\route\09 API\session1\demo\E_Commerce\E_Commerce.API\bin\Debug\net8.0\DataSeed\products.json
                //main of path              // will return base directory
                var seedRoot = Path.Combine(AppContext.BaseDirectory, "DataSeed");
               await SeedIfEmptyAsync<ProductBrand,int>(seedRoot, "brands.json",c);
                await SeedIfEmptyAsync<ProductType, int>(seedRoot, "types.json", c);
                await SeedIfEmptyAsync<Product, int>(seedRoot, "products.json", c);
                await SeedIfEmptyAsync<DeliveryMethod, int>(seedRoot, "delivery.json", c);

                int result =  await  dbContext.SaveChangesAsync(c);
                if (result > 0)
                    logger.LogInformation($"{result} Rows Added");
                else
                  logger.LogInformation("Database Already Seeded");

            }
            catch { }
            

         
        }
        private async Task SeedIfEmptyAsync<T,Tkey>(string rootPath,string fileName,CancellationToken c) where T :BaseEntity<Tkey>
        {
            if (await dbContext.Set<T>().AnyAsync())
            {
                logger.LogInformation("Table Already Has Data");
                return; 
            }

            var filePath=Path.Combine(rootPath, fileName);

            if (!File.Exists(filePath))
            {
                logger.LogWarning($"File {fileName} Is Not Found");
                return; 
            }
            //open streem with path to let Deserialize reed from it
            using var fileStram=File.OpenRead(filePath);

            var options = new JsonSerializerOptions() {PropertyNameCaseInsensitive=true };
                             //to avoid block threed > finish Deserializtion and send  obj to work on it
         var items=  await JsonSerializer.DeserializeAsync<List<T>>(fileStram,options,c); // ex:return list of brands
            //if there is no data to see
            if (items?.Any() ?? false)//items?.conut>0 [not very efficient] 
              dbContext.Set<T>().AddRange(items);//add loacally
            
        }
    }
}

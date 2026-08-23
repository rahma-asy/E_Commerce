using E_Commerce.Domain.Contracts;

namespace E_Commerce.API.Extentions
{
    public static class WebAppExtentions
    {
        public static async Task<WebApplication> SeedAndMigrateDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");//have obj of IDataSeeder and CatalogDataSeeder   

            var Identityseeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");//have obj of IDataSeeder and CatalogDataSeeder   

            await seeder.SeedDataAsync();
            await Identityseeder.SeedDataAsync();

            return app;
        }
    }
}

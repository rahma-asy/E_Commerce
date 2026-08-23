using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Identity.Data;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding
{
    internal class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext       _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole>    _roleManager;
        private readonly ILogger<IdentityDataSeeder>  _logger;

        public IdentityDataSeeder(StoreIdentityDbContext dbContext,
                                  UserManager<ApplicationUser> userManager,
                                  RoleManager<IdentityRole> roleManager, ILogger<IdentityDataSeeder> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task SeedDataAsync(CancellationToken c = default)
        {
            try
            {
                var PendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(c);//return collection of pending migrations's name
                if (PendingMigrations.Any()) await _dbContext.Database.MigrateAsync(c);

                if(!await _roleManager.Roles.AnyAsync(c))
                {
                  await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

                }


                if(! await _userManager.Users.AnyAsync(c))
                {

                    var admin = new ApplicationUser()
                    {
                        DisplayName = "Rahma Hossam",
                        Email = "rahmaasy14@gmail.com",
                        UserName = "rahmaHossam",
                        PhoneNumber = "12365485692"
                    };
                    var result= await _userManager.CreateAsync(admin,"P@ssw0rd");

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(admin, "SuperAdmin");
                    }
                    else
                    {
                        var errors = string.Join(";",result.Errors.Select(e => e.Description));
                        _logger.LogWarning($"Can Not Seed Defult Admin {errors}");
                    }
                    //    var superAdmin = new ApplicationUser()
                    //    {
                    //        DisplayName = "Rahma Hossam",
                    //        Email = "RahmaHossam14",
                    //        UserName = "rahmaHossam",
                    //        PhoneNumber = "12365485692"
                    //    };
                    //await _userManager.CreateAsync(superAdmin, "P@ssw0rd");
                }

            }
            catch(Exception e) 
            {
                _logger.LogError(e, "Identity Data Seeding Failed");
                return;
            }

        }
    }
}

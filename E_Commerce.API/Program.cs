
using E_Commerce.API.Extentions;
using E_Commerce.Application;
using E_Commerce.Application.Comman;
using E_Commerce.Application.Profiles;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure;
using E_Commerce.Infrastructure.Identity.Entities;
using E_Commerce.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            #region  session 1

            //builder.Services as first parameter for AddInfrastructureServices
            //InfrastructureServiceRegistration.AddInfrastructureServices(builder.Services,builder.Configuration);

            builder.Services.AddInfrastructureServices(builder.Configuration);
            #endregion
            builder.Services.AddApplicationServices();
            builder.Services.Configure<UrlSettings>(builder.Configuration.GetSection("UrlStrings"));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            #region session 6,8
            //will Configure JWTSettings opptions from JWT  
            builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));
            builder.Services.Configure<PaymentGatewaySettings>(builder.Configuration.GetSection("Stripe"));

            #endregion
            builder.Services.AddEndpointsApiExplorer();
            //  builder.Services.AddSwaggerGen();

            #region Swagger 
           

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });
            #endregion



            var app = builder.Build();
            #region session2
            //1-if i call CatalogDataSeeder here i must have obj from StoreDbContext & CatalogDataSeeder &Ilogger   
            //2-will let clr enject it > InfrastructureServiceRegesteration
            //3-create extention from App to create scope

            await app.SeedAndMigrateDataAsync();

            #endregion

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseStaticFiles(new StaticFileOptions { 
            FileProvider=new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath,"Files")),
            RequestPath="/Files" //whem will do this 
            });
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

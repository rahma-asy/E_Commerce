using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.DataSeeding;
using E_Commerce.Infrastructure.Identity.Data;
using E_Commerce.Infrastructure.Identity.Entities;
using E_Commerce.Infrastructure.Identity.Services;
using E_Commerce.Infrastructure.Payments;
using E_Commerce.Infrastructure.Reposatories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure
{
    //external assembly because we want:each layer responsible for regester his services in container and extend i service_collection[by extention method(this)]
   //
    public static class InfrastructureServiceRegesteration
    {
        // اعتبر الميثود دي كأنها ميثود موجودة جوه IServiceCollection.
        
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)//don't have Builder.configuration so he will send it ,to read from appsittengs
        {
            #region session 1 add connection
            services.AddDbContext<StoreDbContext>(options =>
               {
                   options.UseSqlServer(configuration.GetConnectionString("DefultConnection"));//services.AddScoped<StoreDbContext>();will add this
               });
            //session 5
            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));//services.AddScoped<StoreDbContext>();will add this
            });
            #endregion

            #region session 2  seeding
            services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog");//will have more than 1 implementation for IDataSeeder
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //session5
            services.AddKeyedScoped<IDataSeeder, IdentityDataSeeder>("Identity");

            #endregion

            #region session 4 Redis
            // we need one object and one connection during project life time[no need for tracking changes for each request]
            services.AddSingleton<IConnectionMultiplexer>(configurations=>
            {
                //Connect is helper method to create connection obj 
                //
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);

            });
            services.AddScoped<IBasketReposatory, BasketReposatory>();
            services.AddSingleton<ICacheReposatory, CacheReposatory>();
            #endregion


            #region session5 vedio 7 ,8
            //  builder.Services.AddIdentity<ApplicationUser, IdentityRole>();//[useful] in add service in mvc because it is rejester for full identity
            //[role manger-signin manger-user manger - hashing password-cookie identity -cookie authentication-any any thing related by UI] 
            // ==========================Here we do:-
            services.AddIdentityCore<ApplicationUser>()//only basics [[user manger - hashing password...]لو عايز حاجات تانيه ضيفها انت
           .AddRoles<IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();
            //or 
            //.AddRoleManager<RoleManager<IdentityRole>>() ;
            services.AddScoped<IIdintityService, IdintityService>();

            #endregion
            #region session 6
            services.AddScoped<ITokenService, TokenService>();

            var jwtSettings = configuration.GetSection("JWT").Get<JWTSettings>()??throw new InvalidOperationException("JWT Settings is Missing");
            services.AddAuthentication(op => //add all Authentication services & specifies  default AuthenticationScheme
            {
                //middelware know that he will handle Authentication by JWT
                //2 op used to ensure that someone is Authenticated
                op.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;//Reed token from endpoint and compare=>have[have autoriz and token]
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//if user is not Authenticated => will handel requst that is't have token

            }).AddJwtBearer(op=>
            {
                //after validation success save token is aethenticatuin propertity [later you can get it directlly by http context]
                op.SaveToken = true;//HttpContext.GettoxenAsync("access token");and  use it
                op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
        ValidateIssuer = true,
        ValidIssuer= jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience= jwtSettings.Audience,
        ValidateLifetime = true,
        RequireExpirationTime=true,//must have
        ValidateIssuerSigningKey = true,
        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ClockSkew =TimeSpan.FromMinutes(5)//If there's a small difference in time let it pass[extra time]

                };
            });
            #endregion
            services.AddSingleton<IPaymentGateway, StripePaymentGateway>();// objدايما نفس ال  [static ]
            return services;
        }  
    }
}

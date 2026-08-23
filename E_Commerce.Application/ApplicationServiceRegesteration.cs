using E_Commerce.Application.Contracts;
using E_Commerce.Application.Profiles;
using E_Commerce.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application
{
    public static class ApplicationServiceRegesteration
    {
        //call in program line 26
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // services.AddAutoMapper(c => c.AddProfiles(new[]{new ProductProfile(), new ProductProfile() })); //add more than 1 profile
            //scan asembly and add to   services.AddAutoMapper any calss that inhert from profile
            services.AddAutoMapper(c => { },typeof(ApplicationServiceRegesteration).Assembly);
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddSingleton<ICacheService, CacheService>();   
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();   



       return services;
        }
    }
}

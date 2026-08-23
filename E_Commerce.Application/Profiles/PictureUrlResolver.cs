using AutoMapper;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    public class PictureUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly UrlSettings _settings;

        public PictureUrlResolver(IOptions<UrlSettings> options)
        {
            _settings = options.Value;
        }
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            //sourse=>images/products/ClassicWhiteTShirt.jpeg
            //return=>https://localhost:7243/Files/images/products/ClassicWhiteTShirt.jpeg
            var baseUrl = _settings.BaseUrl.TrimEnd('/');
            var path = source.PictureUrl.TrimStart('/');
            return $"{baseUrl}/Files/{path}";
        }
    }
    public class UrlSettings
    {
        public string BaseUrl { get; set; }
    }
}

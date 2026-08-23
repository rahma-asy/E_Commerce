using AutoMapper;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Domain.Entities.Orders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    internal class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly UrlSettings _settings;

        public OrderItemPictureUrlResolver(IOptions<UrlSettings> options)
        {
            _settings = options.Value;
        }

        public string Resolve(OrderItem source,OrderItemDto destination,string destMember,ResolutionContext context)
        {
            var baseUrl = _settings.BaseUrl.TrimEnd('/');
            var path = source.ProductItemOrdered.PictureUrl.TrimStart('/');
            return $"{baseUrl}/Files/{path}";
        }
    }
}

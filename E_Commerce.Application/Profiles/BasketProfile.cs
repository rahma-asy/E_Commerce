using AutoMapper;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Domain.Entities.Baskets;


namespace E_Commerce.Application.Profiles
{
    internal class BasketProfile:Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketDto,CustomerBasket>().ReverseMap();
             CreateMap<BasketItem,BasketItemDto>().ReverseMap();//the navegation property
        }
    }
}

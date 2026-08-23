using AutoMapper;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Domain.Entities.Products;

namespace E_Commerce.Application.Profiles
{
    internal class ProductProfile:Profile
    {
        public ProductProfile() 
        {
            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType, TypeDto>();
            CreateMap<Product, ProductDto>().
                ForMember(des => des.ProductBrand, options => options.MapFrom(srs => srs.ProductBrand.Name))
                .ForMember(des => des.ProductType, options => options.MapFrom(srs => srs.ProductType.Name))
            .ForMember(des => des.PictureUrl, op => op.MapFrom<PictureUrlResolver>());

        }

    }
}

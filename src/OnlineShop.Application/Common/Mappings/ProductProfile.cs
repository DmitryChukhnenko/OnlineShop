using AutoMapper;
using OnlineShop.Application.Products.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Categories, 
                opt => opt.MapFrom(src => 
                    src.ProductCategories
                       .Select(pc => pc.Category.Name)
                       .ToList()));

        CreateMap<CreateProductCommand, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());
    }
}
using AutoMapper;
using OnlineShop.Application.ProductReviews.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Mappings;

public class ProductReviewProfile : Profile
{
    public ProductReviewProfile()
    {
        CreateMap<ProductReview, ProductReviewDTO>()
            .ForMember(x => x.Product,
                opt => opt.MapFrom(x => x.Product))
            .ForMember(x => x.User,
                opt => opt.MapFrom(x => x.User));
        
        CreateMap<CreateProductReviewCommand, ProductReview>();
    }
}
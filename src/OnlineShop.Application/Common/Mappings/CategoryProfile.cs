using AutoMapper;
using OnlineShop.Application.Categories.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDTO>()
            .ForMember(d => d.ChildCategories, 
                o => o.MapFrom(s => s.ChildCategories.Select(c => c.Id)));
        
        CreateMap<CreateCategoryCommand, Category>();
    }
}
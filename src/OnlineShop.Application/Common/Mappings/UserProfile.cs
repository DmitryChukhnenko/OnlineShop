using AutoMapper;
using OnlineShop.Application.Users.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserDTO>();
        
        CreateMap<CreateUserCommand, ApplicationUser>();
    }
}
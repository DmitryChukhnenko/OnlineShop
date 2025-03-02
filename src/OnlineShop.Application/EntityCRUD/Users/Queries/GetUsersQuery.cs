using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.DTOs;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Infrastructure.Users.Queries;

public record GetUsersQuery() : IRequest<List<ApplicationUserDTO>>;

// Query Handler
public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<ApplicationUserDTO>>
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUsersQueryHandler(
        IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<List<ApplicationUserDTO>> Handle(
        GetUsersQuery request, 
        CancellationToken cancellationToken)
    {
        return await _userManager.Users
            .ProjectTo<ApplicationUserDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
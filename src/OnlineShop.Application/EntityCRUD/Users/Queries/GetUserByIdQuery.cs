using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Infrastructure.Users.Queries;


public record GetUserByIdQuery(Guid Id) : IRequest<ApplicationUserDTO?>;

// Query Handler
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApplicationUserDTO?>
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserByIdQueryHandler(
        IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ApplicationUserDTO?> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ArgumentException("Id is required");

        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            throw new ArgumentException("User not found");

        return _mapper.Map<ApplicationUserDTO>(user);
    }
}
using MediatR;
using OnlineShop.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Application.Users.Commands;

public record CreateUserCommand(
    string UserName,
    string Email,
    string Password,
    string ConfirmPassword) : IRequest<IdentityResult>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IdentityResult>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Password != request.ConfirmPassword) {
            return IdentityResult.Failed(new IdentityError
                { Description = "Passwords do not match" });
        }
        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email            
        };
        
        return await _userManager.CreateAsync(user, request.Password);
    }
}
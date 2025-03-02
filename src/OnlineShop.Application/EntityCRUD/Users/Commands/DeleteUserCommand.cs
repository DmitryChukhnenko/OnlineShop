using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Users.Commands;

public record DeleteUserCommand(Guid Id) : IRequest<IdentityResult>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, IdentityResult>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null) {
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }
        return await _userManager.DeleteAsync(user);
    }
}
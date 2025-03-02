using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Users.Commands;

public record UpdateUserCommand(
    Guid Id,
    string FullName,
    string Email,
    string PhoneNumber,
    string Address,
    string Password,
    string NewPassword
    ) : IRequest<IdentityResult>;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, IdentityResult>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        user.FullName = request.FullName;
        user.Address = request.Address;

        var numberToken = await _userManager
            .GenerateChangeEmailTokenAsync(user, request.Email);
        var emailToken = await _userManager
            .GenerateChangePhoneNumberTokenAsync(user, request.PhoneNumber);

        await _userManager.ChangePhoneNumberAsync(user,
            request.PhoneNumber, numberToken);
        await _userManager.ChangeEmailAsync(user,
            request.Email, emailToken);    
        await _userManager.ChangePasswordAsync(user,
            request.Password, request.NewPassword);

        return await _userManager.UpdateAsync(user);
    }
}
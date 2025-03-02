using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Users.Commands;

public record LoginCommand(string Email, string Password) : IRequest<SignInResult>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, SignInResult>
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginCommandHandler(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<SignInResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(
            request.Email,
            request.Password,
            isPersistent: false,
            lockoutOnFailure: false);
            

        return result;
    }
}
using FluentValidation;
using OnlineShop.Application.Users.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required")
            .MaximumLength(256).WithMessage("Max length 256 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("ConfirmPassword  is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
    }
}
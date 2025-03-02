using FluentValidation;
using OnlineShop.Application.Users.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}
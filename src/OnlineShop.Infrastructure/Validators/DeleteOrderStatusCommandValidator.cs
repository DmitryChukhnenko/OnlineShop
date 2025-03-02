using FluentValidation;
using OnlineShop.Application.OrderStatuses.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class DeleteOrderStatusCommandValidator : AbstractValidator<DeleteOrderStatusCommand>
{
    public DeleteOrderStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}
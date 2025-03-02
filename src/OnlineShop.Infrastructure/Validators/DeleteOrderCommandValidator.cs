using FluentValidation;
using OnlineShop.Application.Orders.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}
using FluentValidation;
using OnlineShop.Application.Orders.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.ShippingAddress)
            .NotEmpty().WithMessage("ShippingAddress is required")
            .MaximumLength(100);

        RuleFor(x => x.ShippedDate)
            .NotEmpty().WithMessage("ShippedDate is required");
            
        RuleFor(x => x.TotalAmount)
            .NotEmpty().WithMessage("TotalAmount is required");
    }
}
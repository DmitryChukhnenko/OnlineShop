using FluentValidation;
using OnlineShop.Application.Orders.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
            
        RuleFor(x => x.ShippingAddress)
            .NotEmpty().WithMessage("ShippingAddress is required")
            .MaximumLength(100);

        RuleFor(x => x.ShippedDate)
            .NotEmpty().WithMessage("ShippedDate is required");
            
        RuleFor(x => x.TotalAmount)
            .NotEmpty().WithMessage("TotalAmount is required");
    }
}
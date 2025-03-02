using FluentValidation;
using OnlineShop.Application.OrderStatuses.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .IsInEnum();

        RuleFor(x => x.CurrentLocation)
            .NotEmpty().WithMessage("CurrentLocation is required");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("CurrentLocation is required")
            .MaximumLength(300).WithMessage("Max length 300");
    }
}
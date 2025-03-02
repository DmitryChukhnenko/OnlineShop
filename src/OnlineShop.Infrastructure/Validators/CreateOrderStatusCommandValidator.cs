using FluentValidation;
using OnlineShop.Application.OrderStatuses.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class CreateOrderStatusCommandValidator : AbstractValidator<CreateOrderStatusCommand>
{
    public CreateOrderStatusCommandValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .IsInEnum();

        RuleFor(x => x.CurrentLocation)
            .NotEmpty().WithMessage("Current location is required");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(300).WithMessage("Max length 300");
    }
}
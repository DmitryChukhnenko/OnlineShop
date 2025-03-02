using FluentValidation;
using OnlineShop.Application.PaymentDetails.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class DeletePaymentDetailCommandValidator : AbstractValidator<DeletePaymentDetailCommand>
{
    public DeletePaymentDetailCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}
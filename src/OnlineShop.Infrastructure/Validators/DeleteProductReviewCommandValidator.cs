using FluentValidation;
using OnlineShop.Application.ProductReviews.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class DeleteProductReviewCommandValidator : AbstractValidator<DeleteProductReviewCommand>
{
    public DeleteProductReviewCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}
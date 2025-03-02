using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ProductReviews.Commands;
using OnlineShop.Infrastructure.Data;

namespace OnlineShop.Infrastructure.Validators;

public class UpdateProductReviewCommandValidator : AbstractValidator<UpdateProductReviewCommand>
{
    public UpdateProductReviewCommandValidator(ApplicationDbContext context)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
                    
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(100);

        RuleFor(x => x.Rate)
            .NotEmpty().WithMessage("Rate is required");

        // RuleFor(x => x.User)
        //     .NotEmpty().WithMessage("User is required")
        //     .MustAsync(async (User, _) => 
        //         !await context.ProductReviews.AnyAsync(c => c.Id == User.Id));

        RuleFor(x => x.Product)
            .NotEmpty().WithMessage("Product is required")
            .MustAsync(async (Product, _) => 
                !await context.ProductReviews.AnyAsync(c => c.Equals(Product)));
    }
}
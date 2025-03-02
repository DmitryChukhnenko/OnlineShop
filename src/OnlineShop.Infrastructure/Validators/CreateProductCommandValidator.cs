using FluentValidation;
using OnlineShop.Application.Products.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Max length 200 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.StockQuantity)
            .GreaterThan(0).WithMessage("Stock quantity must be greater than 0");

        RuleFor(x => x.SKU)
            .NotEmpty()
            .Matches(@"^[A-Z0-9-]+$");

        RuleFor(x => x.CategoryIds)
            .NotEmpty().WithMessage("At least one category required")
            .Must(c => c.Distinct().Count() == c.Count)
            .WithMessage("Duplicate categories are not allowed");
    }
}
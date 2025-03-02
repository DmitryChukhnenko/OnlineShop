using FluentValidation;
using OnlineShop.Application.Products.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}
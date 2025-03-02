using FluentValidation;
using OnlineShop.Application.Categories.Commands;

namespace OnlineShop.Infrastructure.Validators;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}
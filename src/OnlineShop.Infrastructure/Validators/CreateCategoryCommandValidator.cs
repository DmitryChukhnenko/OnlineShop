using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Categories.Commands;
using OnlineShop.Infrastructure.Data;

namespace OnlineShop.Infrastructure.Validators;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(ApplicationDbContext context)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100);

        RuleFor(x => x.Slug)
            .NotEmpty()
            .Matches("^[a-z0-9-]+$")
            .MustAsync(async (slug, _) => 
                !await context.Categories.AnyAsync(c => c.Slug == slug))
            .WithMessage("Slug must be unique");
    }
}
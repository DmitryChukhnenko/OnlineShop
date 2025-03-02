using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Application.Categories.Commands;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    string Slug,    
    Guid? ParentCategoryId) : IRequest<Unit>;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.Query()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (category == null) {
            throw new ArgumentException("Category not found");
        }

        category.Name = request.Name;
        category.Slug = request.Slug;
        if (request.ParentCategoryId.HasValue)
            category.ParentCategoryId = request.ParentCategoryId;

        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
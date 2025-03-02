using MediatR;
using OnlineShop.Domain.Entities;
using AutoMapper;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Application.Categories.Commands;

public record CreateCategoryCommand(
    string Name,
    string Slug,
    Guid? ParentCategoryId) : IRequest<Guid>;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(
        CreateCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(request);

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.CommitAsync();
        
        return category.Id;
    }
}
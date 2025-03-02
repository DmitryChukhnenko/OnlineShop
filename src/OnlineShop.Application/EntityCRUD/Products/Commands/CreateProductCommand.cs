using AutoMapper;
using MediatR;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string SKU,
    string ImageUrl,
    ICollection<Guid> CategoryIds) : IRequest<Guid>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRepository<Category> _categoryRepository;

    public CreateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRepository<Category> categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<Guid> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request);
        
        // Добавляем категории
        foreach (var categoryId in request.CategoryIds)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId)!;
            if (category != null)
            {
                product.ProductCategories.Add(new ProductCategory
                {
                    Product = product,
                    Category = category
                });
            }
        }

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.CommitAsync();
        
        return product.Id;
    }
}
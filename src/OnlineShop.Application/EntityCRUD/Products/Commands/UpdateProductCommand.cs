using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Products.Commands;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string SKU,
    string ImageUrl,
    ICollection<Guid> CategoryIds) : IRequest<Unit>;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRepository<Category> _categoryRepository;

    public UpdateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRepository<Category> categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.Query()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (product == null) {
            throw new ArgumentException("Product not found");
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        product.SKU = request.SKU;
        product.ImageUrl = request.ImageUrl;
        product.ProductCategories = [];

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

        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
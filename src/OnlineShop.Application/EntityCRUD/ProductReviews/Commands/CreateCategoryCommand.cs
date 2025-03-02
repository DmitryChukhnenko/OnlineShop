using MediatR;
using OnlineShop.Domain.Entities;
using AutoMapper;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Application.ProductReviews.Commands;

public record CreateProductReviewCommand(
    int Rate,
    string Description,
    ApplicationUserDTO User,
    ProductDTO Product 
    ) : IRequest<Guid>;

public class CreateProductReviewCommandHandler : IRequestHandler<CreateProductReviewCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductReviewCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(
        CreateProductReviewCommand request, 
        CancellationToken cancellationToken)
    {
        var ProductReview = _mapper.Map<ProductReview>(request);

        await _unitOfWork.ProductReviews.AddAsync(ProductReview);
        await _unitOfWork.CommitAsync();
        
        return ProductReview.Id;
    }
}
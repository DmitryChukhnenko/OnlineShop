using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Application.ProductReviews.Commands;

public record UpdateProductReviewCommand(
    Guid Id,
    int Rate,
    string Description,
    ApplicationUserDTO User,
    ProductDTO Product 
    ) : IRequest<Unit>;

public class UpdateProductReviewCommandHandler : IRequestHandler<UpdateProductReviewCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductReviewCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateProductReviewCommand request, CancellationToken cancellationToken)
    {
        var ProductReview = await _unitOfWork.ProductReviews.Query()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (ProductReview == null) {
            throw new ArgumentException("ProductReview not found");
        }

        ProductReview.Rate = request.Rate;
        ProductReview.Description = request.Description;
        ProductReview.User = _mapper.Map<Domain.Entities.ApplicationUser>(request.User);
        ProductReview.Product = _mapper.Map<Domain.Entities.Product>(request.Product);

        await _unitOfWork.ProductReviews.UpdateAsync(ProductReview);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
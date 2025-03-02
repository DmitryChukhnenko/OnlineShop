using MediatR;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Application.ProductReviews.Commands;

public record DeleteProductReviewCommand(Guid Id) : IRequest<Unit>;

public class DeleteProductReviewCommandHandler : IRequestHandler<DeleteProductReviewCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteProductReviewCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ProductReviews.SoftDeleteAsync(request.Id);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
using MediatR;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Application.PaymentDetails.Commands;

public record DeletePaymentDetailCommand(Guid Id) : IRequest<Unit>;

public class DeletePaymentDetailCommandHandler : IRequestHandler<DeletePaymentDetailCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePaymentDetailCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeletePaymentDetailCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.PaymentDetails.SoftDeleteAsync(request.Id);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
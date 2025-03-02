using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Application.PaymentDetails.Commands;

public record UpdatePaymentDetailCommand(
    Guid Id,
    decimal Amount,
    string Currency,
    string CardNumber,
    string CardHolderName,
    DateTime ExpirationDate,
    string CVV,
    Guid OrderId
    ) : IRequest<Unit>;

public class UpdatePaymentDetailCommandHandler : IRequestHandler<UpdatePaymentDetailCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePaymentDetailCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdatePaymentDetailCommand request, CancellationToken cancellationToken)
    {
        var PaymentDetail = await _unitOfWork.PaymentDetails.Query()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (PaymentDetail == null) {
            throw new ArgumentException("PaymentDetail not found");
        }

        PaymentDetail.Amount = request.Amount;
        PaymentDetail.Currency = request.Currency;
        PaymentDetail.CardNumber = request.CardNumber;
        PaymentDetail.CardHolderName = request.CardHolderName;
        PaymentDetail.ExpirationDate = request.ExpirationDate;
        PaymentDetail.OrderId = request.OrderId;

        await _unitOfWork.PaymentDetails.UpdateAsync(PaymentDetail);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
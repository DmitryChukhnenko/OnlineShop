using MediatR;
using OnlineShop.Domain.Entities;
using AutoMapper;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Application.PaymentDetails.Commands;

public record CreatePaymentDetailCommand(
    decimal Amount,
    string Currency,
    string CardNumber,
    string CardHolderName,
    DateTime ExpirationDate,
    string CVV,
    Guid OrderId
    ) : IRequest<Guid>;

public class CreatePaymentDetailCommandHandler : IRequestHandler<CreatePaymentDetailCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePaymentDetailCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(
        CreatePaymentDetailCommand request, 
        CancellationToken cancellationToken)
    {
        var PaymentDetail = _mapper.Map<PaymentDetail>(request);

        await _unitOfWork.PaymentDetails.AddAsync(PaymentDetail);
        await _unitOfWork.CommitAsync();
        
        return PaymentDetail.Id;
    }
}
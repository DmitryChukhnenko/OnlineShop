using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Infrastructure.PaymentDetails.Queries;

public record GetPaymentDetailByIdQuery(Guid Id = default) : IRequest<PaymentDetailDTO?>;

// Query Handler
public class GetPaymentDetailByIdQueryHandler : IRequestHandler<GetPaymentDetailByIdQuery, PaymentDetailDTO?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPaymentDetailByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaymentDetailDTO?> Handle(
        GetPaymentDetailByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ArgumentException("Id is required");

        return await _unitOfWork.PaymentDetails.Query()
            .Where(c => c.Id == request.Id)
            .Include(c => c.Order)
            .ProjectTo<PaymentDetailDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
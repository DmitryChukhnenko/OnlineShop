using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Infrastructure.PaymentDetails.Queries;

public record GetPaymentDetailsQuery() : IRequest<List<PaymentDetailDTO>>;

// Query Handler
public class GetPaymentDetailsQueryHandler : IRequestHandler<GetPaymentDetailsQuery, List<PaymentDetailDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaymentDetailsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PaymentDetailDTO>> Handle(
        GetPaymentDetailsQuery request, 
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.PaymentDetails.Query()
            .Include(c => c.Order)
            .ProjectTo<PaymentDetailDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
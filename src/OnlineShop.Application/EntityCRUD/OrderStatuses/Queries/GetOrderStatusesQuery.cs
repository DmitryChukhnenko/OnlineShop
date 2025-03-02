using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Infrastructure.OrderStatuses.Queries;

public record GetOrderStatusesQuery() : IRequest<List<OrderStatusDTO>>;

// Query Handler
public class GetOrderStatusesQueryHandler : IRequestHandler<GetOrderStatusesQuery, List<OrderStatusDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetOrderStatusesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<OrderStatusDTO>> Handle(
        GetOrderStatusesQuery request, 
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.OrderStatuses.Query()
            .ProjectTo<OrderStatusDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
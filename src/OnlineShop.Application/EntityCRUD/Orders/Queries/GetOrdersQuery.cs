using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Infrastructure.Orders.Queries;

public record GetOrdersQuery() : IRequest<List<OrderDTO>>;

// Query Handler
public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetOrdersQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<OrderDTO>> Handle(
        GetOrdersQuery request, 
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.Orders.Query()
            .Include(c => c.Items)
            .ProjectTo<OrderDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
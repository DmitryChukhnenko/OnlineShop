using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Infrastructure.OrderStatuses.Queries;

public record GetOrderStatusByIdQuery(Guid Id = default) : IRequest<OrderStatusDTO?>;

// Query Handler
public class GetOrderStatusByIdQueryHandler : IRequestHandler<GetOrderStatusByIdQuery, OrderStatusDTO?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrderStatusByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderStatusDTO?> Handle(
        GetOrderStatusByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ArgumentException("Id is required");

        return await _unitOfWork.OrderStatuses.Query()
            .Where(c => c.Id == request.Id)
            .ProjectTo<OrderStatusDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
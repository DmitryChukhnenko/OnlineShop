using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Infrastructure.Orders.Queries;

public record GetOrderByIdQuery(Guid Id = default) : IRequest<OrderDTO?>;

// Query Handler
public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDTO?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderDTO?> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ArgumentException("Id is required");

        return await _unitOfWork.Orders.Query()
            .Where(c => c.Id == request.Id)
            .Include(c => c.Items)
            .ThenInclude(c => c.Product)
            .ProjectTo<OrderDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
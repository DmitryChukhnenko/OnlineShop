using MediatR;
using OnlineShop.Domain.Entities;
using AutoMapper;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Application.Orders.Commands;

public record CreateOrderCommand(
    decimal TotalAmount,
    string ShippingAddress,
    DateTime? ShippedDate
    ) : IRequest<Guid>;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(
        CreateOrderCommand request, 
        CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(request);

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.CommitAsync();
        
        return order.Id;
    }
}
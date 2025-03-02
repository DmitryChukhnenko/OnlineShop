using MediatR;
using OnlineShop.Domain.Entities;
using AutoMapper;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Application.OrderStatuses.Commands;

public record CreateOrderStatusCommand(
    OrderStatusEnum Status,
    string Description,
    string CurrentLocation) : IRequest<Guid>;

public class CreateOrderStatusCommandHandler : IRequestHandler<CreateOrderStatusCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(
        CreateOrderStatusCommand request, 
        CancellationToken cancellationToken)
    {
        var OrderStatus = _mapper.Map<OrderStatus>(request);

        await _unitOfWork.OrderStatuses.AddAsync(OrderStatus);
        await _unitOfWork.CommitAsync();
        
        return OrderStatus.Id;
    }
}
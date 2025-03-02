using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.OrderStatuses.Commands;

public record UpdateOrderStatusCommand(
    Guid Id,
    OrderStatusEnum Status,
    string Description,
    string CurrentLocation) : IRequest<Unit>;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOrderStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var OrderStatus = await _unitOfWork.OrderStatuses.Query()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (OrderStatus == null) {
            throw new ArgumentException("OrderStatus not found");
        }

        OrderStatus.Status = (int)request.Status;
        OrderStatus.Description = request.Description;
        OrderStatus.CurrentLocation = request.CurrentLocation;

        await _unitOfWork.OrderStatuses.UpdateAsync(OrderStatus);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
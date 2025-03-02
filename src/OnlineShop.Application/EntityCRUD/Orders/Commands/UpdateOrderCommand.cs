using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Orders.Commands;

public record UpdateOrderCommand(
    Guid Id,
    decimal TotalAmount,
    string ShippingAddress,
    DateTime? ShippedDate
    ) : IRequest<Unit>;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.Query()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (order == null) {
            throw new ArgumentException("Order not found");
        }

        order.TotalAmount = request.TotalAmount;
        order.ShippingAddress = request.ShippingAddress;
        order.ShippedDate = request.ShippedDate;

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
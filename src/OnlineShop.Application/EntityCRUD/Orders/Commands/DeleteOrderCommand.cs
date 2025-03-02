using MediatR;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Application.Orders.Commands;

public record DeleteOrderCommand(Guid Id) : IRequest<Unit>;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Orders.SoftDeleteAsync(request.Id);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
using MediatR;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Application.OrderStatuses.Commands;

public record DeleteOrderStatusCommand(Guid Id) : IRequest<Unit>;

public class DeleteOrderStatusCommandHandler : IRequestHandler<DeleteOrderStatusCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteOrderStatusCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.OrderStatuses.SoftDeleteAsync(request.Id);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Application.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<Unit>;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Products.SoftDeleteAsync(request.Id);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
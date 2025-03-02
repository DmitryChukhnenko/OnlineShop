using MediatR;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Application.Categories.Commands;

public record DeleteCategoryCommand(Guid Id) : IRequest<Unit>;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Categories.SoftDeleteAsync(request.Id);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
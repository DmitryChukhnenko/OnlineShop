using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Infrastructure.Categories.Queries;

public record GetCategoryByIdQuery(Guid Id = default) : IRequest<CategoryDTO?>;

// Query Handler
public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDTO?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryDTO?> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ArgumentException("Id is required");

        return await _unitOfWork.Categories.Query()
            .Where(c => c.Id == request.Id)
            .Include(c => c.ChildCategories)
            .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
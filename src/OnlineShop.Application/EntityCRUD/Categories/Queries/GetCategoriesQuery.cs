using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Infrastructure.Categories.Queries;

public record GetCategoriesQuery() : IRequest<List<CategoryDTO>>;

// Query Handler
public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoriesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CategoryDTO>> Handle(
        GetCategoriesQuery request, 
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.Categories.Query()
            .Include(c => c.ChildCategories)
            .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
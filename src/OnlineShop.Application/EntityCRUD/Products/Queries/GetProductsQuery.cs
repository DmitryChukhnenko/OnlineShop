using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Infrastructure.Products.Queries;

public record GetProductsQuery(
    string SearchTerm = null!, 
    int Page = 1, 
    int PageSize = 10) : IRequest<PagedList<ProductDTO>>;

// Query Handler
public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedList<ProductDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICachingService _cache;

    public GetProductsQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        ICachingService cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<PagedList<ProductDTO>> Handle(
        GetProductsQuery request, 
        CancellationToken cancellationToken)
    {
        var cacheKey = $"products_{request.SearchTerm}_{request.Page}_{request.PageSize}";
        var cached = await _cache.GetAsync<PagedList<ProductDTO>>(cacheKey);
        if (cached != null) return cached;

        var query = _unitOfWork.Products.Query()
            .Where(p => !p.IsDeleted);

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(p => 
                p.Name.Contains(request.SearchTerm) || 
                p.Description.Contains(request.SearchTerm));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<ProductDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var result = new PagedList<ProductDTO>(items, totalCount, request.Page, request.PageSize);
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));

        return result;
    }
}
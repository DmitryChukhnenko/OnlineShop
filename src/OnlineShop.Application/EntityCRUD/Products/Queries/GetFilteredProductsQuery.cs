using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

public record GetFilteredProductsQuery(    
    Filter Filter,
    string SearchTerm = null!, 
    int Page = 1, 
    int PageSize = 10
) : IRequest<PagedList<ProductDTO>>;

public enum ProductSortOrder
{
    PriceAsc,
    PriceDesc,
    Newest,
    Popularity
}

public class Filter {
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<Guid> CategoryIds { get; set; } = new();
    public ProductSortOrder SortOrder { get; set; }
}

public class GetFilteredProductsQueryHandler : IRequestHandler<GetFilteredProductsQuery, PagedList<ProductDTO>> {

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICachingService _cache;

    public GetFilteredProductsQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        ICachingService cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<PagedList<ProductDTO>> Handle(
        GetFilteredProductsQuery request, 
        CancellationToken ct) {        
        
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
        
        // Фильтрация
        if (request.Filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= request.Filter.MinPrice);
        
        if (request.Filter.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= request.Filter.MaxPrice);
        
        if (request.Filter.CategoryIds.Any())
            query = query.Where(p => p.ProductCategories
                .Any(pc => request.Filter.CategoryIds.Contains(pc.CategoryId)));
        
        if (!string.IsNullOrEmpty(request.SearchTerm))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{request.SearchTerm}%"));
        
        // Сортировка
        query = request.Filter.SortOrder switch
        {
            ProductSortOrder.PriceAsc => query.OrderBy(p => p.Price),
            ProductSortOrder.PriceDesc => query.OrderByDescending(p => p.Price),
            ProductSortOrder.Newest => query.OrderByDescending(p => p.CreatedAt),
            ProductSortOrder.Popularity => query
                .OrderByDescending(p => p.OrderItems.Sum(oi => oi.Quantity)),
            _ => query.OrderBy(p => p.Name)
        };

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
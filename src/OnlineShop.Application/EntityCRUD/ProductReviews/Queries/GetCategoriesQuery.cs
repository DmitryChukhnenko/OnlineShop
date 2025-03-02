using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Infrastructure.ProductReviews.Queries;

public record GetProductReviewsQuery() : IRequest<List<ProductReviewDTO>>;

// Query Handler
public class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQuery, List<ProductReviewDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetProductReviewsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ProductReviewDTO>> Handle(
        GetProductReviewsQuery request, 
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.ProductReviews.Query()
            .Include(c => c.User)
            .Include(c => c.Product)
            .ProjectTo<ProductReviewDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
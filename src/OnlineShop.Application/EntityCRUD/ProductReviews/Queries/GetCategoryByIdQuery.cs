using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.DTOs;

namespace OnlineShop.Infrastructure.ProductReviews.Queries;

public record GetProductReviewByIdQuery(Guid Id = default) : IRequest<ProductReviewDTO?>;

// Query Handler
public class GetProductReviewByIdQueryHandler : IRequestHandler<GetProductReviewByIdQuery, ProductReviewDTO?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductReviewByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductReviewDTO?> Handle(
        GetProductReviewByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ArgumentException("Id is required");

        return await _unitOfWork.ProductReviews.Query()
            .Where(c => c.Id == request.Id)
            .Include(c => c.User)
            .Include(c => c.Product)
            .ProjectTo<ProductReviewDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
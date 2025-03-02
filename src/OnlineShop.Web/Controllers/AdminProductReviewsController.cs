
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.ProductReviews.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Infrastructure.ProductReviews.Queries;

namespace OnlineShop.Web.Components.Controllers;

[Route("api/admin/productreviews")]
//[Authorize(Roles = "Admin")]
public class AdminProductReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminProductReviewsController(IMediator mediator) 
        => _mediator = mediator;

    [HttpGet]
    public async Task<List<ProductReviewDTO>> GetProductReviews(
        [FromQuery] GetProductReviewsQuery query) 
        => await _mediator.Send(query);

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductReviewDTO>> GetProductReview(Guid id)
    {
        var ProductReview = await _mediator.Send(new GetProductReviewByIdQuery(id));
        return ProductReview != null ? Ok(ProductReview) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateProductReview(
        [FromBody] CreateProductReviewCommand command)
    {
        var ProductReviewId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProductReview), new { id = ProductReviewId }, ProductReviewId);
    }

    [HttpPut]
    public async Task<ActionResult<Guid>> UpdateProductReview(
        [FromBody] UpdateProductReviewCommand command)
    {
        var ProductReviewId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProductReview), new { id = ProductReviewId }, ProductReviewId);
    }

    [HttpDelete]
    public async Task<ActionResult<Guid>> DeleteProductReview(
        [FromBody] DeleteProductReviewCommand command)
    {
        var ProductReviewId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProductReview), new { id = ProductReviewId }, ProductReviewId);
    }
}
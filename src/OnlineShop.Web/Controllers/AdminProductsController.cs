
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Common;
using OnlineShop.Application.Products.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Infrastructure.Products.Queries;

namespace OnlineShop.Web.Components.Controllers;

[Route("api/admin/products")]
//[Authorize(Roles = "Admin")]
public class AdminProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminProductsController(IMediator mediator) 
        => _mediator = mediator;

    [HttpGet]
    public async Task<PagedList<ProductDTO>> GetProducts(
        [FromQuery] GetProductsQuery query) 
        => await _mediator.Send(query);

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(Guid id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        return product != null ? Ok(product) : NotFound();
    }

    [HttpGet("filtered")]
    public async Task<PagedList<ProductDTO>> GetFilteredProducts(
        [FromQuery] GetFilteredProductsQuery query) 
        => await _mediator.Send(query);

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateProduct(
        [FromBody] CreateProductCommand command)
    {
        var productId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProduct), new { id = productId }, productId);
    }

    [HttpPut]
    public async Task<ActionResult<Guid>> UpdateProduct(
        [FromBody] UpdateProductCommand command)
    {
        var productId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProduct), new { id = productId }, productId);
    }

    [HttpDelete]
    public async Task<ActionResult<Guid>> DeleteProduct(
        [FromBody] DeleteProductCommand command)
    {
        var productId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProduct), new { id = productId }, productId);
    }
}
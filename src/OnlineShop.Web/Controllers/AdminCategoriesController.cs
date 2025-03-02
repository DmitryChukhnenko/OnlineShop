
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Categories.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Infrastructure.Categories.Queries;

namespace OnlineShop.Web.Components.Controllers;

[Route("api/admin/categories")]
//[Authorize(Roles = "Admin")]
public class AdminCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminCategoriesController(IMediator mediator) 
        => _mediator = mediator;

    [HttpGet]
    public async Task<List<CategoryDTO>> GetCategories(
        [FromQuery] GetCategoriesQuery query) 
        => await _mediator.Send(query);

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDTO>> GetCategory(Guid id)
    {
        var Category = await _mediator.Send(new GetCategoryByIdQuery(id));
        return Category != null ? Ok(Category) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateCategory(
        [FromBody] CreateCategoryCommand command)
    {
        var CategoryId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCategory), new { id = CategoryId }, CategoryId);
    }

    [HttpPut]
    public async Task<ActionResult<Guid>> UpdateCategory(
        [FromBody] UpdateCategoryCommand command)
    {
        var CategoryId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCategory), new { id = CategoryId }, CategoryId);
    }

    [HttpDelete]
    public async Task<ActionResult<Guid>> DeleteCategory(
        [FromBody] DeleteCategoryCommand command)
    {
        var CategoryId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCategory), new { id = CategoryId }, CategoryId);
    }
}
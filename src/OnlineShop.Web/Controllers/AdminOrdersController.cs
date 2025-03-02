
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Orders.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Infrastructure.Orders.Queries;

namespace OnlineShop.Web.Components.Controllers;

[Route("api/admin/orders")]
//[Authorize(Roles = "Admin")]
public class AdminOrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminOrdersController(IMediator mediator) 
        => _mediator = mediator;

    [HttpGet]
    public async Task<List<OrderDTO>> GetOrders(
        [FromQuery] GetOrdersQuery query) 
        => await _mediator.Send(query);

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(Guid id)
    {
        var Order = await _mediator.Send(new GetOrderByIdQuery(id));
        return Order != null ? Ok(Order) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateOrder(
        [FromBody] CreateOrderCommand command)
    {
        var OrderId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrder), new { id = OrderId }, OrderId);
    }

    [HttpPut]
    public async Task<ActionResult<Guid>> UpdateOrder(
        [FromBody] UpdateOrderCommand command)
    {
        var OrderId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrder), new { id = OrderId }, OrderId);
    }

    [HttpDelete]
    public async Task<ActionResult<Guid>> DeleteOrder(
        [FromBody] DeleteOrderCommand command)
    {
        var OrderId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrder), new { id = OrderId }, OrderId);
    }
}
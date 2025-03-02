
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.OrderStatuses.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Infrastructure.OrderStatuses.Queries;

namespace OnlineShop.Web.Components.Controllers;

[Route("api/admin/orderstatuses")]
//[Authorize(Roles = "Admin")]
public class AdminOrderStatusesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminOrderStatusesController(IMediator mediator) 
        => _mediator = mediator;

    [HttpGet]
    public async Task<List<OrderStatusDTO>> GetOrderStatuses(
        [FromQuery] GetOrderStatusesQuery query) 
        => await _mediator.Send(query);

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderStatusDTO>> GetOrderStatus(Guid id)
    {
        var OrderStatus = await _mediator.Send(new GetOrderStatusByIdQuery(id));
        return OrderStatus != null ? Ok(OrderStatus) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateOrderStatus(
        [FromBody] CreateOrderStatusCommand command)
    {
        var OrderStatusId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrderStatus), new { id = OrderStatusId }, OrderStatusId);
    }

    [HttpPut]
    public async Task<ActionResult<Guid>> UpdateOrderStatus(
        [FromBody] UpdateOrderStatusCommand command)
    {
        var OrderStatusId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrderStatus), new { id = OrderStatusId }, OrderStatusId);
    }

    [HttpDelete]
    public async Task<ActionResult<Guid>> DeleteOrderStatus(
        [FromBody] DeleteOrderStatusCommand command)
    {
        var OrderStatusId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrderStatus), new { id = OrderStatusId }, OrderStatusId);
    }
}
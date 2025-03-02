
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.PaymentDetails.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Infrastructure.PaymentDetails.Queries;

namespace OnlineShop.Web.Components.Controllers;

[Route("api/admin/paymentdetails")]
//[Authorize(Roles = "Admin")]
public class AdminPaymentDetailsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminPaymentDetailsController(IMediator mediator) 
        => _mediator = mediator;

    [HttpGet]
    public async Task<List<PaymentDetailDTO>> GetPaymentDetails(
        [FromQuery] GetPaymentDetailsQuery query) 
        => await _mediator.Send(query);

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDetailDTO>> GetPaymentDetail(Guid id)
    {
        var PaymentDetail = await _mediator.Send(new GetPaymentDetailByIdQuery(id));
        return PaymentDetail != null ? Ok(PaymentDetail) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePaymentDetail(
        [FromBody] CreatePaymentDetailCommand command)
    {
        var PaymentDetailId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPaymentDetail), new { id = PaymentDetailId }, PaymentDetailId);
    }

    [HttpPut]
    public async Task<ActionResult<Guid>> UpdatePaymentDetail(
        [FromBody] UpdatePaymentDetailCommand command)
    {
        var PaymentDetailId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPaymentDetail), new { id = PaymentDetailId }, PaymentDetailId);
    }

    [HttpDelete]
    public async Task<ActionResult<Guid>> DeletePaymentDetail(
        [FromBody] DeletePaymentDetailCommand command)
    {
        var PaymentDetailId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPaymentDetail), new { id = PaymentDetailId }, PaymentDetailId);
    }
}
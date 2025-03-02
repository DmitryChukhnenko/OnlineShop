
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Users.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Infrastructure.Users.Queries;

namespace OnlineShop.Web.Components.Controllers;

[Route("api/admin/users")]
//[Authorize(Roles = "Admin")]
public class AdminUsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminUsersController(IMediator mediator) 
        => _mediator = mediator;

    [HttpGet]
    public async Task<List<ApplicationUserDTO>> GetUsers(
        [FromQuery] GetUsersQuery query) 
        => await _mediator.Send(query);

    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicationUserDTO>> GetUser(Guid id)
    {
        var User = await _mediator.Send(new GetUserByIdQuery(id));
        return User != null ? Ok(User) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<IdentityResult>> CreateUser(
        [FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPut]
    public async Task<ActionResult<IdentityResult>> UpdateUser(
        [FromBody] UpdateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpDelete]
    public async Task<ActionResult<Guid>> DeleteUser(
        [FromBody] DeleteUserCommand command)
    {
        var UserId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUser), new { id = UserId }, UserId);
    }
}
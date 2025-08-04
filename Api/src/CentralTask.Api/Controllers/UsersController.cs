using CentralTask.Application.Commands.UserCommands.CriarUserCommand;
using CentralTask.Application.Commands.UserCommands.RealizarLoginCommand;
using CentralTask.Application.Queries.UserQueries.ObterTodosUserQuery;
using CentralTask.Core.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralTask.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : BaseController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("createRandom")]
    public async Task<IActionResult> CriarUser([FromBody] CriarUserCommandInput input)
    {
        return HandleResult(await _mediator.Send(input));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] RealizarLoginCommandInput input)
    {
        return HandleResult(await _mediator.Send(input));
    }

    [HttpGet()]
    [Authorize]
    public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUserQueryInput input)
    {
        return HandleResult(await _mediator.Send(input));
    }
}
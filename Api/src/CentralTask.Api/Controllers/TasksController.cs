using CentralTask.Application.Commands.TasksCommands;
using CentralTask.Application.Queries.TasksQueries;
using CentralTask.Core.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralTask.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TasksController : BaseController
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost()]
    [Authorize]
    public async Task<IActionResult> CriarTasks([FromBody] CriarTasksCommandInput input)
    {
        return HandleResult(await _mediator.Send(input));
    }

    [HttpPut()]
    [Authorize]
    public async Task<IActionResult> AlterarTasks([FromBody] AlterarTasksCommandInput input)
    {
        return HandleResult(await _mediator.Send(input));
    }

    [HttpDelete()]
    [Authorize]
    public async Task<IActionResult> DeletarTasks([FromBody] DeletarTasksCommandInput input)
    {
        return HandleResult(await _mediator.Send(input));
    }

    [HttpGet("by-id")]
    [Authorize]
    public async Task<IActionResult> GetByIdTasks([FromQuery] GetByIdTasksInput input)
    {
        return HandleResult(await _mediator.Send(input));
    }

    [HttpGet("paginado")]
    [Authorize]
    public async Task<IActionResult> GetPaginadoTasks([FromQuery] GetPaginadoTasksInput input)
    {
        return HandleResult(await _mediator.Send(input));
    }
}
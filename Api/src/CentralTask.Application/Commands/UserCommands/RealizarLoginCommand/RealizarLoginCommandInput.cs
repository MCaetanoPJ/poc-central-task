using CentralTask.Core.Mediator.Commands;

namespace CentralTask.Application.Commands.UserCommands.RealizarLoginCommand;

public class RealizarLoginCommandInput : CommandInput<RealizarLoginCommandResult>
{
    public string Email { get; set; }
    public string Senha { get; set; }
}
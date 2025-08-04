using CentralTask.Core.Mediator.Commands;

namespace CentralTask.Application.Commands.UserCommands.CriarUserCommand;

public class CriarUserCommandInput : CommandInput<CriarUserCommandResult>
{
    public int Amount { get; set; }
    public string UserNameMask { get; set; }
}
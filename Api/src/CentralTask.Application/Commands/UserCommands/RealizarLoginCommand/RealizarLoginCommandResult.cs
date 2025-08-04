using CentralTask.Core.Mediator.Commands;

namespace CentralTask.Application.Commands.UserCommands.RealizarLoginCommand;

public class RealizarLoginCommandResult : CommandResult
{
    public string AccessToken { get; set; }
    public double ExpiresInSeconds { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public Guid? UserId { get; set; }
    public int Nivel { get; set; }
}
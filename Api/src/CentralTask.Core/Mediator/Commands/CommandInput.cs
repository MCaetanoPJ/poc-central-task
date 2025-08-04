namespace CentralTask.Core.Mediator.Commands;

public class CommandInput<TCommandResult> : MediatorInput<TCommandResult> where TCommandResult : CommandResult
{
}
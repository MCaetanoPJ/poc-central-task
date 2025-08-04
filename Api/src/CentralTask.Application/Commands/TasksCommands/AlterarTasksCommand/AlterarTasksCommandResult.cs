
using CentralTask.Core.Mediator.Commands;

namespace CentralTask.Application.Commands.TasksCommands
{
    public class AlterarTasksCommandResult : CommandResult
    {
        public Guid? Id { get; set; }
    }
}
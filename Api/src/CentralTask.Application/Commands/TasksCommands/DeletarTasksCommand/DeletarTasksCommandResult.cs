
using CentralTask.Core.Mediator.Commands;

namespace CentralTask.Application.Commands.TasksCommands
{
    public class DeletarTasksCommandResult : CommandResult
    {
        public Guid? Id { get; set; }
    }
}
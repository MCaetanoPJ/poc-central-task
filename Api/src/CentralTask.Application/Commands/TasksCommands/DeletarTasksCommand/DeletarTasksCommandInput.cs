
using CentralTask.Core.Mediator.Commands;

namespace CentralTask.Application.Commands.TasksCommands
{
    public class DeletarTasksCommandInput : CommandInput<DeletarTasksCommandResult>
    {
        public Guid Id { get; set; }
    }
}

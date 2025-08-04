
using CentralTask.Core.Mediator.Commands;

namespace CentralTask.Application.Commands.TasksCommands
{
    public class CriarTasksCommandResult : CommandResult
    {
        public Guid? Id { get; set; }
    }
}
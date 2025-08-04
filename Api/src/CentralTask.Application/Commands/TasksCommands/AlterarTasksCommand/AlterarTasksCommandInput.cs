using CentralTask.Core.Mediator.Commands;
using CentralTask.Domain.Enums;

namespace CentralTask.Application.Commands.TasksCommands
{
    public class AlterarTasksCommandInput : CommandInput<AlterarTasksCommandResult>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public StatusTask Status { get; set; }
        public Guid UserId { get; set; }
    }
}

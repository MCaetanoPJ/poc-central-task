
using CentralTask.Core.Mediator.Commands;
using CentralTask.Domain.Enums;

namespace CentralTask.Application.Commands.TasksCommands
{
    public class CriarTasksCommandInput : CommandInput<CriarTasksCommandResult>
    {
        public string Title { get; set; }
        public StatusTask Status { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid UserId { get; set; }
    }
}

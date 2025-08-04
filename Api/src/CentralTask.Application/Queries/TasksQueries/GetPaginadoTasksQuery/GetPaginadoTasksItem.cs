
using CentralTask.Domain.Enums;

namespace CentralTask.Application.Queries.TasksQueries
{
    public class GetPaginadoTasksItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid UserId { get; set; }
        public int Status { get; set; }
    }
}
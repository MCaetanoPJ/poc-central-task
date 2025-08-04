namespace CentralTask.Application.Queries.TasksQueries
{
    public class GetByIdTasksItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid UserId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
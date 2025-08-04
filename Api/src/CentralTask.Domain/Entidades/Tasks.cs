
using CentralTask.Domain.Entidades.Base;
using CentralTask.Domain.Enums;

namespace CentralTask.Domain.Entidades
{
    public class Tasks : Entidade
    {
        public Tasks() { }

        public Tasks(string title, string description, DateTime dueDate, Guid userId, User user)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            UserId = userId;
            User = user;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public StatusTask Status { get; set; }
        public DateTime DueDate { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
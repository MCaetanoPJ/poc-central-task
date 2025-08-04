
using CentralTask.Domain.Entidades;
using CentralTask.Domain.Interfaces.Repositories;
using CentralTask.Infra.Data.Context;
using CentralTask.Infra.Data.Repositories.Base;

namespace CentralTask.Infra.Data.Repositories
{
    public class TasksRepository : GenericRepository<Tasks>, ITasksRepository
    {
        public TasksRepository(CentralTaskContext context) : base(context)
        {
        }
    }
}
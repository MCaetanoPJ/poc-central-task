using CentralTask.Domain.Entidades;
using CentralTask.Domain.Interfaces.Repositories.Base;

namespace CentralTask.Domain.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    public Task<bool> UserExiste(Guid id);
    Task<List<User>> GetAllAsync();
}

using CentralTask.Domain.Entidades;
using CentralTask.Domain.Interfaces.Repositories;
using CentralTask.Infra.Data.Context;
using CentralTask.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CentralTask.Infra.Data.Repositories;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(CentralTaskContext context) : base(context)
    {
    }

    public Task<bool> UserExiste(Guid id)
    {
        return GetAsNoTracking().AnyAsync(c => c.Id == id);
    }
    public async Task<List<User>> GetAllAsync()
    {
        return GetAsNoTracking().ToList();
    }
}
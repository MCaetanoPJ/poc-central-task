using CentralTask.Core.RepositoryBase;

namespace CentralTask.Application.Services.Interfaces;

public interface IUserService
{
    Task<ValidateResult> DeletarUser(Guid UserId);
}

using CentralTask.Application.Services.Interfaces;
using CentralTask.Core.RepositoryBase;
using CentralTask.Domain.Interfaces.Repositories;

namespace CentralTask.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _UserRepository;

    public UserService(
        IUserRepository UserRepository)
    {
        _UserRepository = UserRepository;
    }

    public async Task<ValidateResult> DeletarUser(Guid UserId)
    {
        var validacao = new ValidateResult();

        var User = _UserRepository
            .Get()
            .Where(c => c.Id == UserId)
            .FirstOrDefault();

        if (User == null)
        {
            validacao.AddMessage("Usuário informado não foi encontrado em nossa base de dados.");
            return validacao;
        }

        _UserRepository.Remove(User);
        await _UserRepository.UnitOfWork.SaveChangesAsync();

        return validacao;
    }
}
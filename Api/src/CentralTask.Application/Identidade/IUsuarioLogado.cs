using CentralTask.Domain.Enums;

namespace CentralTask.Application.Identidade;

public interface IUserLogado
{
    bool EstaLogado();
    Guid? ObterId();
    string ObterToken();
    EnumNivel? ObterNivel();
}

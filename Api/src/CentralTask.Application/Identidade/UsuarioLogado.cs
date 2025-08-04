using CentralTask.Core.Extensions;
using CentralTask.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CentralTask.Application.Identidade;

public class UserLogado : IUserLogado
{
    private readonly IHttpContextAccessor _acessor;

    public UserLogado(IHttpContextAccessor acessor)
    {
        _acessor = acessor;
    }

    public bool EstaLogado()
    {
        return _acessor.HttpContext.EstaAutenticado();
    }

    public string ObterToken()
    {
        return _acessor.HttpContext.ObterBearerToken();
    }

    public Guid? ObterId()
    {
        return Guid.TryParse(_acessor.HttpContext.ObterUserId(), out var guidId)
            ? guidId
            : null;
    }

    public EnumNivel? ObterNivel()
    {
        var nivel = _acessor.HttpContext.ObterPerfil();

        if (string.IsNullOrEmpty(nivel)) return null;

        Enum.TryParse<EnumNivel>(nivel, true, out var nivelEnum);

        return nivelEnum;
    }
}

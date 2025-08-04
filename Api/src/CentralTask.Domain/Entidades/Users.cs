using CentralTask.Domain.Entidades.Base;
using CentralTask.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CentralTask.Domain.Entidades;

public class User : IdentityUser<Guid>, IEntidade
{
    public User(string nome, string email)
    {
        ArgumentNullException.ThrowIfNull(nome);

        Id = Guid.NewGuid();
        Nome = nome;
        Email = UserName = email.ToLower().Trim();
    }

    public User()
    {
    }

    public string Nome { get; set; }
    public Status Active { get; set; }
    public EnumNivel NivelAcesso { get; set; }
}
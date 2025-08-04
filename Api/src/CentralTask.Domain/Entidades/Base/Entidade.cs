using CentralTask.Domain.Enums;

namespace CentralTask.Domain.Entidades.Base;

public abstract class Entidade : IEntidade
{
    protected Entidade()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        Active = Status.Ativo;
    }
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; set; }
    public Status Active { get; set; }

    public void AtivarInativar()
    {
        if (Active == Status.Ativo)
        {
            Active = Status.Inativo;
        }
        else if (Active == Status.Inativo)
        {
            Active = Status.Ativo;
        }
    }
}
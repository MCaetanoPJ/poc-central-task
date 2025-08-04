using CentralTask.Domain.Enums;

namespace CentralTask.Domain.Entidades.Base;

public interface IEntidade
{
    public Guid Id { get; set; }
    public Status Active { get; set; }

}
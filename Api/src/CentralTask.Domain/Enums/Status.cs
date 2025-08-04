using System.ComponentModel;

namespace CentralTask.Domain.Enums;

public enum Status
{
    [Description("Ativo")]
    Ativo,

    [Description("Inativo")]
    Inativo
}
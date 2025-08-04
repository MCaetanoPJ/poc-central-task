using CentralTask.Core.Mediator.Commands;
using FluentValidation;

namespace CentralTask.Application.Commands.UserCommands.RealizarLoginCommand;

public class RealizarLoginCommandInputValidator : CommandInputValidator<RealizarLoginCommandInput>
{
    public RealizarLoginCommandInputValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(300);

        RuleFor(x => x.Senha)
            .NotEmpty()
            .MaximumLength(36);
    }
}


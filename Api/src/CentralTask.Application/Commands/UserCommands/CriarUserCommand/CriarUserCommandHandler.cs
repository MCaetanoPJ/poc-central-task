using CentralTask.Core.Mediator.Commands;
using CentralTask.Core.Notifications;
using CentralTask.Domain.Entidades;
using CentralTask.Domain.Enums;
using CentralTask.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CentralTask.Application.Commands.UserCommands.CriarUserCommand;

public class CriarUserCommandHandler : ICommandHandler<CriarUserCommandInput, CriarUserCommandResult>
{
    private readonly ILogger<CriarUserCommandHandler> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly INotifier _notifier;

    public CriarUserCommandHandler(
        ILogger<CriarUserCommandHandler> logger,
        UserManager<User> userManager,
        IUserRepository userRepository,
        INotifier notifier)
    {
        _logger = logger;
        _userManager = userManager;
        _userRepository = userRepository;
        _notifier = notifier;
    }

    public async Task<CriarUserCommandResult> Handle(CriarUserCommandInput request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0)
        {
            _notifier.Notify("A quantidade de usuários a serem criados deve ser maior que zero.");
            return new CriarUserCommandResult();
        }

        if (string.IsNullOrEmpty(request.UserNameMask))
        {
            _notifier.Notify("Obrigatório informar a máscara para o nome dos usuários a serem criados.");
            return new CriarUserCommandResult();
        }

        for (int position = 0; position < request.Amount; position++)
        {
            var name = request.UserNameMask.Replace("{{random}}", Guid.NewGuid().ToString());

            var user = new User(name, email: $"{name}@gmail.com");
            var result = await _userManager.CreateAsync(user, "123456");
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    continue;
                }
            }

            var levelAccess = EnumNivel.Admin.ToString();
            var rolesCurrentToUser = await _userManager.GetRolesAsync(user);
            var resultRole = await _userManager.AddToRoleAsync(user, levelAccess);
            if (!resultRole.Succeeded)
            {
                foreach (var error in resultRole.Errors)
                {
                    continue;
                }
            }
        }

        return new();
    }
}
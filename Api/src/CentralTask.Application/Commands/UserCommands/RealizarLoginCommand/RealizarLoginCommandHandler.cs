using CentralTask.Application.Services.Interfaces;
using CentralTask.Core.Mediator.Commands;
using CentralTask.Core.Notifications;
using CentralTask.Core.Settings;
using CentralTask.Domain.Entidades;
using CentralTask.Domain.Enums;
using CentralTask.Domain.Interfaces.Repositories;
using ChoETL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CentralTask.Application.Commands.UserCommands.RealizarLoginCommand;

public class RealizarLoginCommandHandler : ICommandHandler<RealizarLoginCommandInput, RealizarLoginCommandResult>
{
    private readonly ILogger<RealizarLoginCommandHandler> _logger;
    private readonly IUserRepository _UserAppRepository;
    private readonly IUserRepository _UserRepository;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly INotifier _notifier;
    private readonly JwtSettings _jwtSettings;
    private readonly IUserService _UserService;

    public RealizarLoginCommandHandler(
        ILogger<RealizarLoginCommandHandler> logger,
        IUserRepository UserAppRepository,
        IUserRepository UserRepository,
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IOptions<JwtSettings> jwtOptions,
        IConfiguration configuration,
        INotifier notifier,
        IUserService UserService)
    {
        _logger = logger;
        _UserAppRepository = UserAppRepository;
        _UserRepository = UserRepository;
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtSettings = jwtOptions.Value;
        _configuration = configuration;
        _notifier = notifier;
        _UserService = UserService;
    }

    public async Task<RealizarLoginCommandResult> Handle(RealizarLoginCommandInput request, CancellationToken cancellationToken)
    {
        var email = request.Email.ToLower().Trim();

        var signInResult = await _signInManager.PasswordSignInAsync(email, request.Senha, false, false);

        if (!signInResult.Succeeded)
        {
            _notifier.Notify("E-mail ou senha incorretos.");
            return new RealizarLoginCommandResult();
        }

        var user = _UserRepository.Get().Where(x => x.Email.ToLower().Trim() == email).FirstOrDefault();

        _logger.LogInformation("Usuário {UserId} logado com sucesso.", user.Id);

        return await GerarReponseComToken(user);
    }

    public async Task<RealizarLoginCommandResult> GerarReponseComToken(User user)
    {
        var encodedToken = CriarToken(user);
        var responses = new RealizarLoginCommandResult
        {
            AccessToken = encodedToken,
            ExpiresInSeconds = TimeSpan.FromHours(_jwtSettings.ExpiracaoHoras).TotalSeconds,
            Nivel = ((int)user.NivelAcesso),
            Email = user.Email,
            Nome = user.Nome,
            UserId = user.Id,
        };

        return responses;
    }

    private string CriarToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var claims = new List<Claim>
        {
            new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject
            new Claim(ClaimTypes.Name, user.Nome),
            new Claim(ClaimTypes.Role, user.NivelAcesso.ToString()),
            new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID único do token
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = ObterTempoDeExpiracao(user.NivelAcesso),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            NotBefore = DateTime.UtcNow,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static DateTime? ObterTempoDeExpiracao(EnumNivel nivelAcesso)
    {
        return nivelAcesso switch
        {
            EnumNivel.Admin => DateTime.Now.AddHours(5),

            _ => throw new NotImplementedException(),
        };
    }
}
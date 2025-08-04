using CentralTask.Core.AppSettingsConfigurations;
using CentralTask.Core.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;

namespace CentralTask.Api.Extensions;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };

            x.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Erro de autenticação: {context.Exception.Message}");
                    return Task.CompletedTask;
                },

                //OnMessageReceived = context =>
                //{
                //    var accessToken = context.Request.Query["access_token"];

                //    var path = context.HttpContext.Request.Path;
                //    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notifications"))
                //    {
                //        context.Token = accessToken;
                //    }
                //    return Task.CompletedTask;
                //}
            };
        });

        return services;
    }

    public static WebApplication UseSwaggerConfiguration(this WebApplication app, ConfigSwagger configSwagger)
    {
        app.UseWhen(context => context.Request.Path.StartsWithSegments("/swagger"), appBuilder =>
        {
            appBuilder.Use(async (ctx, next) =>
            {
                var authHeader = ctx.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                {
                    var token = authHeader.Substring("Basic ".Length).Trim();
                    try
                    {
                        var credentialString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                        var credentials = credentialString.Split(':', 2);

                        var reqUser = credentials[0];
                        var reqPass = credentials.Length > 1 ? credentials[1] : string.Empty;

                        if (reqUser == configSwagger?.User && reqPass == configSwagger?.Senha)
                        {
                            await next();
                            return;
                        }
                    }
                    catch
                    {

                    }
                }

                ctx.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Swagger\"";
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            });
        });

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
            options.DocExpansion(DocExpansion.None);
        });

        return app;
    }
}

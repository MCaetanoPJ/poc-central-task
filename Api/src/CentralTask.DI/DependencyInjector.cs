using CentralTask.Application.AutoMapper;
using CentralTask.Application.Identidade;
using CentralTask.Application.Services;
using CentralTask.Application.Services.Interfaces;
using CentralTask.Broker.Connection;
using CentralTask.Broker.Consumer;
using CentralTask.Broker.Interfaces;
using CentralTask.Broker.Producer;
using CentralTask.Domain.Interfaces.Repositories;
using CentralTask.Infra.Data.Context;
using CentralTask.Infra.Data.Repositories;
using CentralTask.Infra.Notifications.Hubs;
using CentralTask.Infra.Notifications.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace CentralTask.DI;

public static class DependencyInjector
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddSingleton<Migrator>()
            .AddScoped<IUserLogado, UserLogado>()
            .AddRepositories()
            .AddSharedServices()
            .AddAutoMapper(typeof(AutoMapperConfig))
            .AddDbContext(configuration)
        .AddHttpClient();

        return services;
    }

    public static IServiceCollection AddServicesIntegrador(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddRepositories()
            .AddAutoMapper(typeof(AutoMapperConfig))
            .AddDbContext(configuration, ServiceLifetime.Transient);

        return services;
    }

    public static void SetLocalCulture(this IServiceCollection services)
    {
        var cultureInfo = new CultureInfo("pt-BR");
        cultureInfo.NumberFormat.CurrencySymbol = "R$";

        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }

    private static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<IEventPublisher, EventPublisher>();
        services.AddSingleton<IEventConsumer, EventConsumer>();
        services.AddSingleton<IEventConnection, EventConnection>();
        services.AddHostedService<EventConsumerBackgroundService>();
        services.AddSingleton<IUserConnectionService, UserConnectionService>();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITasksRepository, TasksRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<CentralTaskContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }, serviceLifetime, serviceLifetime);

        return services;
    }
}

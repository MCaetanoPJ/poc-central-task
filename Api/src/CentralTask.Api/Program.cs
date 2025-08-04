using CentralTask.Api.Extensions;
using CentralTask.Broker.Consumer;
using CentralTask.Core.AppSettingsConfigurations;
using CentralTask.Infra.Notifications.Hubs;
using Newtonsoft.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

// Configuração do Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

// Configuração padrão da API
builder.Services.AddApiServicesConfiguration(builder.Configuration);

builder.Services.Configure<ConfigBroker>(builder.Configuration.GetSection("ConfigBroker"));

var app = builder.Build();

app.UseWebSockets();

app.UseApiConfiguration(builder.Configuration);

app.Run();
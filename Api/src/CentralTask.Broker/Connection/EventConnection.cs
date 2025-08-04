using CentralTask.Broker.Interfaces;
using CentralTask.Core.AppSettingsConfigurations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CentralTask.Broker.Connection
{
    public class EventConnection : IEventConnection
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private readonly ConfigBroker _config;

        public EventConnection(IOptions<ConfigBroker> options)
        {
            var config = options.Value;

            _factory = new ConnectionFactory
            {
                HostName = config.HostName,
                Port = config.Port,
                UserName = config.UserName,
                Password = config.Password
            };
        }

        public async Task<IChannel> CreateChannelAsync()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = await _factory.CreateConnectionAsync();
            }

            return await _connection.CreateChannelAsync();
        }
    }
}
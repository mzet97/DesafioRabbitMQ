using Desafio.ProtocoloAPI.Infrastructure.Extensions;
using Desafio.ProtocoloAPI.Infrastructure.MessageBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Desafio.ProtocoloAPI.Infrastructure.Configuration;

public static class MessageBusConfig
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionFactory = new ConnectionFactory();

        var rabbitMqSection = configuration.GetSection("RabbitMq");
        services.Configure<RabbitMq>(rabbitMqSection);
        var rabbitMq = rabbitMqSection.Get<RabbitMq>();

        if (rabbitMq == null)
        {
            throw new ArgumentNullException("RabbitMq configuration is missing");
        }

        //Console.WriteLine($"RabbitMq configuration: HostName: {rabbitMq.HostName}, Port: {rabbitMq.Port},  UserName: {rabbitMq.Username}");
        connectionFactory.HostName = rabbitMq.HostName;
        connectionFactory.Port = rabbitMq.Port;
        connectionFactory.UserName = rabbitMq.Username;
        connectionFactory.Password = rabbitMq.Password;

        var connection = connectionFactory.CreateConnection("protocolos");

        services.AddSingleton(new ProducerConnection(connection));
        services.AddSingleton<IMessageBusClient, RabbitMqClient>();

        return services;
    }
}

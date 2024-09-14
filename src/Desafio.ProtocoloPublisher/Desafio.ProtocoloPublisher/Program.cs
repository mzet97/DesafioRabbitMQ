using Desafio.ProtocoloPublisher.Core.Fakes;
using Desafio.ProtocoloPublisher.Core.Models;
using Desafio.ProtocoloPublisher.Core.Validates;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class Program
{
    public static void Main(string[] args)
    {

        var configuration = new ConfigurationBuilder()
           .SetBasePath(AppContext.BaseDirectory)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true)
           .AddEnvironmentVariables()
           .Build();

        var rabbitMQSettings = new RabbitMQSettings
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME") ?? configuration["RabbitMQ:HostName"],
            Port = int.TryParse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"), out var port) ? port : int.Parse(configuration["RabbitMQ:Port"]),
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? configuration["RabbitMQ:UserName"],
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? configuration["RabbitMQ:Password"],
            QueueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE_NAME") ?? configuration["RabbitMQ:QueueName"]
        };

        if (!ValidateRabbitMQSettings.Validate(rabbitMQSettings))
        {
            Console.WriteLine("Configurações do RabbitMQ estão incompletas ou inválidas.");
            return;
        }

        try
        {
            var protocolos = FakeProtocolo.GetRandom();

            var factory = new ConnectionFactory
            {
                HostName = rabbitMQSettings.HostName,
                Port = rabbitMQSettings.Port,
                UserName = rabbitMQSettings.UserName,
                Password = rabbitMQSettings.Password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: rabbitMQSettings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            foreach (var protocolo in protocolos)
            {
                var json = JsonSerializer.Serialize(protocolo);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: rabbitMQSettings.QueueName, basicProperties: null, body: body);
                Console.WriteLine($"Protocolo {protocolo.NumeroProtocolo} enviado para a fila.");
            }

            Console.WriteLine("Finalizado com sucesso");
        }
        catch (Exception ex)
        {
            Console.WriteLine(rabbitMQSettings);
            Console.WriteLine($"Erro ao enviar protocolos: {ex.Message}");
            Console.WriteLine($"Erro ao enviar protocolos: {ex.InnerException?.Message}");
        }
    }

    
}

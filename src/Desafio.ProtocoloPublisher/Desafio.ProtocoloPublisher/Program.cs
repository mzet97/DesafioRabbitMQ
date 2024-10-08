﻿using Desafio.ProtocoloPublisher.Core.Fakes;
using Desafio.ProtocoloPublisher.Core.Models;
using Desafio.ProtocoloPublisher.Core.Validates;
using Desafio.ProtocoloPublisher.Infrastructure.MessageBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;

public class Program
{
    public static void Main(string[] args)
    {

        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.AddOpenTelemetry(options =>
                {
                    options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("ConsoleAppService"));
                    options.AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri("http://otel-collector:4317");
                    });
                });
            })
            .ConfigureServices((context, services) =>
            {
                services.AddOpenTelemetry()
                     .WithTracing(builder =>
                     {
                         builder
                             .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("ConsoleAppService"))
                             .AddOtlpExporter(options =>
                             {
                                 options.Endpoint = new Uri("http://otel-collector:4317");
                             });
                     });
            })
            .Build();

        var logger = host.Services.GetRequiredService<ILogger<Program>>();


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
            logger.LogCritical("Configurações do RabbitMQ estão incompletas ou inválidas.");
            return;
        }

        try
        {
            var protocolos = FakeProtocolo.GetRandom();

            var connectionFactory = new ConnectionFactory
            {
                HostName = rabbitMQSettings.HostName,
                Port = rabbitMQSettings.Port,
                UserName = rabbitMQSettings.UserName,
                Password = rabbitMQSettings.Password
            };

            var connection = connectionFactory.CreateConnection("protocolos");
            var producerConnection = new ProducerConnection(connection);
            var messageBus = new RabbitMqClient(producerConnection);

            foreach (var protocolo in protocolos)
            {
                messageBus.Publish(protocolo, "protocolos.pending", "protocolos", "protocolos_pending_queue");

                Console.WriteLine($"Protocolo {protocolo.NumeroProtocolo} enviado para a fila.");
                logger.LogInformation($"Protocolo {protocolo.NumeroProtocolo} enviado para a fila.");
            }

            Console.WriteLine("Finalizado com sucesso");
            logger.LogInformation("Finalizado com sucesso");
        }
        catch (Exception ex)
        {
            Console.WriteLine(rabbitMQSettings);
            Console.WriteLine($"Erro ao enviar protocolos: {ex.Message}");
            Console.WriteLine($"Erro ao enviar protocolos: {ex.InnerException?.Message}");

            logger.LogError(ex, "Erro ao enviar protocolos: " + rabbitMQSettings);
            logger.LogError(ex.InnerException, "Erro ao enviar protocolos");
            logger.LogCritical("Erro ao enviar protocolos");
        }
    }

}

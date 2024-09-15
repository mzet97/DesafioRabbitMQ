using Desafio.ProtocoloAPI.Application.Dtos.InputModels;
using Desafio.ProtocoloAPI.Core.Entities;
using Desafio.ProtocoloAPI.Core.Repositories;
using Desafio.ProtocoloAPI.Core.Validations;
using Desafio.ProtocoloAPI.Infrastructure.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Desafio.ProtocoloAPI.Application.Features.Protocolos.Subscribers;

public class ProtocoloSubscriber : BackgroundService
{
    private readonly ILogger<ProtocoloSubscriber> _logger;
    private readonly IMessageBusClient _messageBusClient;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly string _queueName = "protocolos_pending_queue";
    private readonly string _exchange = "protocolos";
    private readonly string _routingKey = "protocolos.pending";

    public ProtocoloSubscriber(
        IMessageBusClient messageBusClient,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ProtocoloSubscriber> logger)
    {
        _messageBusClient = messageBusClient;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageBusClient.Subscribe(_queueName, _exchange, _routingKey, OnMessageReceived);

        return Task.CompletedTask;
    }

    public async void OnMessageReceived(string message)
    {
        Console.WriteLine($"Received message: {message}");
        _logger.LogInformation("Received message: {message}", message);

        try
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var protocoloRepository = scope.ServiceProvider.GetRequiredService<IProtocoloRepository>();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var protocolo = JsonSerializer.Deserialize<ProtocoloInput>(message, options);

                bool isApproved =  await ProcessProtocolo(protocolo, protocoloRepository);

                if (isApproved)
                {


                    _messageBusClient.Publish(protocolo, "protocolos.approved", _exchange, "protocolos_finish_queue");
                    Console.WriteLine($"Protocolo {protocolo.NumeroProtocolo} aprovado e enviado para a fila protocolos.approved.");
                    _logger.LogInformation($"Protocolo {protocolo.NumeroProtocolo} aprovado e enviado para a fila protocolos.approved.");
                }
                else
                {
                    _messageBusClient.Publish(protocolo, "protocolos.refused", _exchange, "protocolos_finish_queue");
                    Console.WriteLine($"Protocolo {protocolo.NumeroProtocolo} recusado e enviado para a fila protocolos.refused.");
                    _logger.LogInformation($"Protocolo {protocolo.NumeroProtocolo} recusado e enviado para a fila protocolos.refused.");
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao processar protocolo: {ex.Message}");
            _logger.LogError(ex, "Erro ao processar protocolo: {message}", message);
        }
    }

    public async Task<bool> ProcessProtocolo(ProtocoloInput? protocolo, IProtocoloRepository protocoloRepository)
    {
        if(protocolo is null)
        {
            return false;
        }

        var entity = new Protocolo(
            protocolo.NumeroProtocolo,
            protocolo.NumeroVia,
            protocolo.Cpf,
            protocolo.Rg,
            protocolo.Nome,
            protocolo.NomeMae,
            protocolo.NomePai,
            protocolo.GetFotoBytes()
        );

        if (!Validator.Validate(new ProtocoloValidation(), entity))
        {
            return false;
        }

        var protolocoEqualNumero = await protocoloRepository.Find(x => x.NumeroProtocolo == entity.NumeroProtocolo);

        if(protolocoEqualNumero.Any())
        {
            return false;
        }

        var protolocoEqualCpf = await protocoloRepository.Find(x => x.Cpf == entity.Cpf && x.NumeroVia == entity.NumeroVia);

        if (protolocoEqualCpf.Any())
        {
            return false;
        }

        var protolocoEqualRg = await protocoloRepository.Find(x => x.Rg == entity.Rg && x.NumeroVia == entity.NumeroVia);

        if (protolocoEqualRg.Any())
        {
            return false;
        }

        await protocoloRepository.Add(entity);

        return true;
    }
}


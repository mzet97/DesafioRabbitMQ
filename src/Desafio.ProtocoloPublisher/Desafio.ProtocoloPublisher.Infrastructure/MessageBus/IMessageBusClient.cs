namespace Desafio.ProtocoloPublisher.Infrastructure.MessageBus;

public interface IMessageBusClient
{
    void Publish(object message, string routingKey, string exchange);
}
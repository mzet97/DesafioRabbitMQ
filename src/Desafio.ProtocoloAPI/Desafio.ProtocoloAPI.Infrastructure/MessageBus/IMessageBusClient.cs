namespace Desafio.ProtocoloAPI.Infrastructure.MessageBus;

public interface IMessageBusClient
{
    void Publish(object message, string routingKey, string exchange, string queueName);
    void Subscribe(string queueName, string exchange, string routingKey, Action<string> onMessageReceived);
}

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Desafio.ProtocoloAPI.Infrastructure.MessageBus;

public class RabbitMqClient : IMessageBusClient
{
    private readonly IConnection _connection;

    public RabbitMqClient(ProducerConnection producerConnection)
    {
        _connection = producerConnection.Connection;
    }

    public void Publish(object message, string routingKey, string exchange)
    {
        var channel = _connection.CreateModel();

        JsonSerializerOptions settings = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var payload = JsonSerializer.Serialize(message, settings);
        var body = Encoding.UTF8.GetBytes(payload);

        channel.ExchangeDeclare(exchange, "topic", true);

        channel.BasicPublish(exchange, routingKey, null, body);
    }

    public void Subscribe(string queueName, string exchange, string routingKey, Action<string> onMessageReceived)
    {
        var channel = _connection.CreateModel();

        channel.ExchangeDeclare(exchange, "topic", true);

        channel.QueueDeclare(queueName, true, false, false, null);

        channel.QueueBind(queueName, exchange, routingKey);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            onMessageReceived(message);
        };

        channel.BasicConsume(queueName, true, consumer);
    }
}


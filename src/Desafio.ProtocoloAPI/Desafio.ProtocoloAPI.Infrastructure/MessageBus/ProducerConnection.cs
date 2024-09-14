using RabbitMQ.Client;

namespace Desafio.ProtocoloAPI.Infrastructure.MessageBus;

public class ProducerConnection
{
    public IConnection Connection { get; private set; }

    public ProducerConnection(IConnection connection)
    {
        Connection = connection;
    }
}
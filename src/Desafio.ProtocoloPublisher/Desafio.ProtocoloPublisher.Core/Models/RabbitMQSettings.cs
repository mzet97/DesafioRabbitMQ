namespace Desafio.ProtocoloPublisher.Core.Models;

public class RabbitMQSettings
{

    public string HostName { get; set; } = "";
    public int Port { get; set; }
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public string QueueName { get; set; } = "";

    public override string ToString()
    {
        return $"HostName: {HostName}, Port: {Port}, UserName: {UserName}, Password: {Password}, QueueName: {QueueName}";
    }
}

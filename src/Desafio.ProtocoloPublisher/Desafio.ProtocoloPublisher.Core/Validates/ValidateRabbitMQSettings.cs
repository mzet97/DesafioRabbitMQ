using Desafio.ProtocoloPublisher.Core.Models;

namespace Desafio.ProtocoloPublisher.Core.Validates;

public static class ValidateRabbitMQSettings
{
    public static bool Validate(RabbitMQSettings settings)
    {
        if (settings == null)
        {
            Console.WriteLine("Configurações do RabbitMQ não encontradas.");
            return false;
        }

        var missingSettings = string.Empty;

        if (string.IsNullOrWhiteSpace(settings.HostName))
            missingSettings += "HostName, ";

        if (settings.Port == 0)
            missingSettings += "Port, ";

        if (string.IsNullOrWhiteSpace(settings.UserName))
            missingSettings += "UserName, ";

        if (string.IsNullOrWhiteSpace(settings.Password))
            missingSettings += "Password, ";

        if (string.IsNullOrWhiteSpace(settings.QueueName))
            missingSettings += "QueueName, ";

        if (!string.IsNullOrEmpty(missingSettings))
        {
            Console.WriteLine($"Configurações ausentes ou inválidas: {missingSettings.TrimEnd(',', ' ')}.");
            return false;
        }

        return true;
    }
}

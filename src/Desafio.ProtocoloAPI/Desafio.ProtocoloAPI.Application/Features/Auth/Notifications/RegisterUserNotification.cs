using MediatR;
using System.Text.Json;

namespace Desafio.ProtocoloAPI.Application.Features.Auth.Notifications;

public class RegisterUserNotification : INotification
{
    public string Email { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public override string? ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

using MediatR;
using Microsoft.Extensions.Logging;

namespace Desafio.ProtocoloAPI.Application.Features.Auth.Notifications.Handlers;

public class AuthNotificationHandler :
                            INotificationHandler<LoginUserNotification>,
                            INotificationHandler<RegisterUserNotification>
{
    private readonly ILogger<AuthNotificationHandler> _logger;

    public AuthNotificationHandler(ILogger<AuthNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(LoginUserNotification notification, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            _logger.LogInformation("LoginUserNotification: " + notification.Message);
        });
    }

    public Task Handle(RegisterUserNotification notification, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            _logger.LogInformation("RegisterUserNotification: " + notification.Message);
        });
    }
}

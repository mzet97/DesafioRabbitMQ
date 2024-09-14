using Desafio.ProtocoloAPI.Application.Features.Auth.ViewModels;
using MediatR;

namespace Desafio.ProtocoloAPI.Application.Features.Auth.Commands;

public class LoginUserCommand : IRequest<LoginResponseViewModel>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
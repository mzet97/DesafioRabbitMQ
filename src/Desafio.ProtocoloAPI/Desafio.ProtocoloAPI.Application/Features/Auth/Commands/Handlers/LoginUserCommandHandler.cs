using Desafio.ProtocoloAPI.Application.Features.Auth.Notifications;
using Desafio.ProtocoloAPI.Application.Features.Auth.ViewModels;
using Desafio.ProtocoloAPI.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Desafio.ProtocoloAPI.Application.Features.Auth.Commands.Handlers;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponseViewModel>
{
    private readonly SignInManager<User> _signInManager;
    private readonly IMediator _mediator;

    public LoginUserCommandHandler(
        SignInManager<User> signInManager,
        IMediator mediator)
    {
        _signInManager = signInManager;
        _mediator = mediator;
    }

    public async Task<LoginResponseViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

        if (result.Succeeded)
        {
            return await _mediator.Send(new GetTokenCommand {  Email = request.Email});
        }
        else if (result.IsLockedOut)
        {
            await _mediator.Publish(new LoginUserNotification { Email = request.Email, Message = "Falha: Login bloqueado" });
            return null;
        }

        await _mediator.Publish(new LoginUserNotification { Email = request.Email, Message = "Falha: Erro ao fazer login" });

        return null;
    }

}

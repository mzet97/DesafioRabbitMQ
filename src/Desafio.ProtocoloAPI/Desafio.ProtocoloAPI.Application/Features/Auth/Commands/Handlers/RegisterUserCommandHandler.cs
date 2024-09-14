using Desafio.ProtocoloAPI.Application.Features.Auth.Notifications;
using Desafio.ProtocoloAPI.Application.Features.Auth.ViewModels;
using Desafio.ProtocoloAPI.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Desafio.ProtocoloAPI.Application.Features.Auth.Commands.Handlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, LoginResponseViewModel>
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;

    public RegisterUserCommandHandler(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IMediator mediator)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<LoginResponseViewModel> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.ToDomain();

        var resultCreateUser = await _userManager.CreateAsync(user, request.Password);

        if (resultCreateUser.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return await _mediator.Send(new GetTokenCommand { Email = request.Email });
        }

        var sb = new StringBuilder();
        foreach (var error in resultCreateUser.Errors)
        {
            sb.Append(error.Description);
        }

        await _mediator.Publish(new RegisterUserNotification { Email = request.Email, Message = sb.ToString() });

        return await _mediator.Send(new GetTokenCommand { Email = request.Email });
    }
}

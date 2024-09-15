using Desafio.ProtocoloAPI.Application.Features.Auth.ViewModels;
using Desafio.ProtocoloAPI.Core.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Desafio.ProtocoloAPI.Application.Features.Auth.Commands;

public class RegisterUserCommand : IRequest<LoginResponseViewModel>
{
    [Required(ErrorMessage = "O campo {0} é requerido")]
    [EmailAddress(ErrorMessage = "O campo {0} é inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é requeridod")]
    [StringLength(255, ErrorMessage = "O campo  {0} deve está entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Password { get; set; }


    public User ToDomain()
    {
        var entity = new User();

        entity.UserName = Email;
        entity.Email = Email;
        entity.EmailConfirmed = true;

        return entity;
    }
}

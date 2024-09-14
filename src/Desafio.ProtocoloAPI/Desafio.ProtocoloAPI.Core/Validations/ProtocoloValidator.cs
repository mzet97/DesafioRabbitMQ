using Desafio.ProtocoloAPI.Core.Entities;
using FluentValidation;

namespace Desafio.ProtocoloAPI.Core.Validations;

public class ProtocoloValidator : AbstractValidator<Protocolo>
{
    public ProtocoloValidator()
    {
        RuleFor(p => p.NumeroProtocolo)
            .NotEmpty().WithMessage("O número do protocolo é obrigatório.")
            .MaximumLength(50).WithMessage("O número do protocolo não pode exceder 50 caracteres.");

        RuleFor(p => p.NumeroVia)
            .NotNull().WithMessage("O número da via é obrigatório.");

        RuleFor(p => p.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Length(11).WithMessage("O CPF deve ter 11 caracteres.");

        RuleFor(p => p.Rg)
            .NotEmpty().WithMessage("O RG é obrigatório.")
            .Length(9).WithMessage("O RG deve ter 9 caracteres.");

        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres.");

        RuleFor(p => p.NomeMae)
            .MaximumLength(100).WithMessage("O nome da mãe não pode exceder 100 caracteres.")
            .When(p => !string.IsNullOrEmpty(p.NomeMae));

        RuleFor(p => p.NomePai)
            .MaximumLength(100).WithMessage("O nome do pai não pode exceder 100 caracteres.")
            .When(p => !string.IsNullOrEmpty(p.NomePai));

        RuleFor(p => p.Foto)
            .NotNull().WithMessage("A foto é obrigatória.");

        RuleFor(p => p.CreatedAt)
            .NotNull().WithMessage("A data de criação é obrigatória.");
    }
}
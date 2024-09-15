using Desafio.ProtocoloAPI.Core.Entities;
using FluentValidation;

namespace Desafio.ProtocoloAPI.Core.Validations;

public class ProtocoloValidation : AbstractValidator<Protocolo>
{
    public ProtocoloValidation()
    {
        RuleFor(p => p.NumeroProtocolo)
            .NotEmpty().WithMessage("O número do protocolo é obrigatório.")
            .MaximumLength(50).WithMessage("O número do protocolo não pode exceder 50 caracteres.");

        RuleFor(p => p.NumeroVia)
            .NotNull().WithMessage("O número da via é obrigatório.");

        RuleFor(p => p.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Length(11).WithMessage("O CPF deve ter 11 caracteres.")
            .Must(ValidoCpf).WithMessage("Inválido {PropertyName}");

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

    protected bool ValidoCpf(string cpf)
    {
        if (string.IsNullOrEmpty(cpf)) return false;

        return IsCpf(cpf);
    }

    public bool IsCpf(string cpf)
    {
        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf;
        string digito;
        int soma;
        int resto;
        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");
        if (cpf.Length != 11)
            return false;
        tempCpf = cpf.Substring(0, 9);
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito = digito + resto.ToString();
        return cpf.EndsWith(digito);
    }
}
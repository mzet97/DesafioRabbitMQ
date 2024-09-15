using Bogus;
using Bogus.Extensions.Brazil;
using Desafio.ProtocoloPublisher.Core.Models;

namespace Desafio.ProtocoloPublisher.Core.Fakes;

public static class FakeProtocolo
{
    public static IEnumerable<Protocolo> GetValid(int qtd)
    {
        var pessoaGenerator = new Faker<Protocolo>("pt_BR")
            .RuleFor(u => u.NumeroProtocolo, (f, u) => f.Random.Guid().ToString())
            .RuleFor(u => u.NumeroVia, (f, u) => f.Random.Int(1, 3))
            .RuleFor(u => u.Cpf, (f, u) => f.Person.Cpf())
            .RuleFor(u => u.Rg, (f, u) => f.Random.ReplaceNumbers("#########"))
            .RuleFor(u => u.Nome, (f, u) => f.Name.FullName())
            .RuleFor(u => u.NomeMae, (f, u) => f.Name.FullName())
            .RuleFor(u => u.NomePai, (f, u) => f.Name.FullName())
            .RuleFor(u => u.Foto, (f, u) => f.Random.Bytes(100));

        return pessoaGenerator.Generate(qtd);
    }

    public static IEnumerable<Protocolo> GetInvalid(int qtd)
    {
        var invalidProtocols = new List<Protocolo>();
        var pessoaGenerator = new Faker<Protocolo>("pt_BR")
            .RuleFor(u => u.NumeroProtocolo, (f, u) => f.Random.Guid().ToString())
            .RuleFor(u => u.NumeroVia, (f, u) => f.Random.Int(1, 3))
            .RuleFor(u => u.Cpf, (f, u) => f.Person.Cpf())
            .RuleFor(u => u.Rg, (f, u) => f.Random.ReplaceNumbers("#########"))
            .RuleFor(u => u.Nome, (f, u) => f.Name.FullName())
            .RuleFor(u => u.NomeMae, (f, u) => f.Name.FullName())
            .RuleFor(u => u.NomePai, (f, u) => f.Name.FullName())
            .RuleFor(u => u.Foto, (f, u) => f.Random.Bytes(100));

        for (int i = 0; i < qtd; i++)
        {
            var protocolo = pessoaGenerator.Generate();

            if (i % 2 == 0)
            {
                protocolo.Cpf = "35465104023";
                protocolo.NumeroVia = 1;
            }

            if (i % 3 == 0)
            {
                protocolo.Rg = "123456789";
                protocolo.NumeroVia = 1;
            }

            if (i % 4 == 0)
            {
                protocolo.NumeroProtocolo = "FBA7163B-8867-4069-8311-305E2E6CBB51";
            }

            if (i % 5 == 0)
            {
                protocolo.Nome = "";
            }

            invalidProtocols.Add(protocolo);
        }

        return invalidProtocols;
    }

    public static IEnumerable<Protocolo> GetRandom()
    {
        var invalid = GetInvalid(5);
        var valid = GetValid(5);

        return invalid.Concat(valid);
    }
}
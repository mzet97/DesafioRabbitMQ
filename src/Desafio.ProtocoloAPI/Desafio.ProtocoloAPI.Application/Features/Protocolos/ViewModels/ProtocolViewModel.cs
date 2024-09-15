using Desafio.ProtocoloAPI.Core.Entities;

namespace Desafio.ProtocoloAPI.Application.Features.Protocolos.ViewModels;

public class ProtocolViewModel
{
    public string NumeroProtocolo { get; set; } = string.Empty;
    public int NumeroVia { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string NomeMae { get; set; } = string.Empty;
    public string NomePai { get; set; } = string.Empty;
    public byte[] Foto { get; set; } = new byte[0];

    public ProtocolViewModel()
    {
    }

    public ProtocolViewModel(string numeroProtocolo, int numeroVia, string cpf, string rg, string nome, string nomeMae, string nomePai, byte[] foto)
    {
        NumeroProtocolo = numeroProtocolo;
        NumeroVia = numeroVia;
        Cpf = cpf;
        Rg = rg;
        Nome = nome;
        NomeMae = nomeMae;
        NomePai = nomePai;
        Foto = foto;
    }

    public ProtocolViewModel(Protocolo protocolo)
    {
        NumeroProtocolo = protocolo.NumeroProtocolo;
        NumeroVia = protocolo.NumeroVia;
        Cpf = protocolo.Cpf;
        Rg = protocolo.Rg;
        Nome = protocolo.Nome;
        NomeMae = protocolo.NomeMae;
        NomePai = protocolo.NomePai;
        Foto = protocolo.Foto;
    }

    public Protocolo ToEntity()
    {
        return new Protocolo(NumeroProtocolo, NumeroVia, Cpf, Rg, Nome, NomeMae, NomePai, Foto);
    }

    public static ProtocolViewModel FromEntity(Protocolo protocolo)
    {
        return new ProtocolViewModel(protocolo);
    }
}

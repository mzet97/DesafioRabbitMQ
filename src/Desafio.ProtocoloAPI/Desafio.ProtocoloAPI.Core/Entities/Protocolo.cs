namespace Desafio.ProtocoloAPI.Core.Entities;

public class Protocolo : AggregateRoot
{
    public string NumeroProtocolo { get; set; } = string.Empty;
    public int NumeroVia { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string NomeMae { get; set; } = string.Empty;
    public string NomePai { get; set; } = string.Empty;
    public byte[] Foto { get; set; } = new byte[0];

    public Protocolo()
    {
    }

    public Protocolo(
        string numeroProtocolo,
        int numeroVia,
        string cpf,
        string rg,
        string nome,
        string nomeMae,
        string nomePai,
        byte[] foto)
    {
        NumeroProtocolo = numeroProtocolo;
        NumeroVia = numeroVia;
        Cpf = cpf.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
        Rg = rg.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
        Nome = nome;
        NomeMae = nomeMae;
        NomePai = nomePai;
        Foto = foto;
    }
}

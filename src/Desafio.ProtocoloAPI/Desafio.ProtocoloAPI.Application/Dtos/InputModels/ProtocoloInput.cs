namespace Desafio.ProtocoloAPI.Application.Dtos.InputModels;

public class ProtocoloInput
{
    public string NumeroProtocolo { get; set; } = string.Empty;
    public int NumeroVia { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string NomeMae { get; set; } = string.Empty;
    public string NomePai { get; set; } = string.Empty;
    public string Foto { get; set; } = string.Empty;

    public byte[] GetFotoBytes() => Convert.FromBase64String(Foto);
}

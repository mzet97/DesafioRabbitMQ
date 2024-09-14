namespace Desafio.ProtocoloPublisher.Core.Models
{
    public class Protocolo
    {
        public string NumeroProtocolo { get; set; } = string.Empty;
        public int NumeroVia { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string Rg { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string NomeMae { get; set; } = string.Empty;
        public string NomePai { get; set; } = string.Empty;
        public byte[] Foto { get; set; } = new byte[0];
    }
}

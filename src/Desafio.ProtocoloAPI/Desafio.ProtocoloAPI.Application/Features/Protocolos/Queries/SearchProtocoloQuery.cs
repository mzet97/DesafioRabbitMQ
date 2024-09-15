using Desafio.ProtocoloAPI.Application.Dtos.InputModels;
using Desafio.ProtocoloAPI.Application.Features.Protocolos.ViewModels;
using Desafio.ProtocoloAPI.Core.Models;
using MediatR;

namespace Desafio.ProtocoloAPI.Application.Features.Protocolos.Queries;

public class SearchProtocoloQuery : BaseSearch, IRequest<BaseResult<ProtocolViewModel>>
{
    public string NumeroProtocolo { get; set; } = string.Empty;
    public int NumeroVia { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string NomeMae { get; set; } = string.Empty;
    public string NomePai { get; set; } = string.Empty;
}

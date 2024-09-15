using Desafio.ProtocoloAPI.Application.Features.Protocolos.ViewModels;
using Desafio.ProtocoloAPI.Core.Entities;
using Desafio.ProtocoloAPI.Core.Models;
using Desafio.ProtocoloAPI.Core.Repositories;
using LinqKit;
using MediatR;

namespace Desafio.ProtocoloAPI.Application.Features.Protocolos.Queries.Handlers;

public class SearchProtocoloQueryHandler : 
    IRequestHandler<SearchProtocoloQuery, BaseResult<ProtocolViewModel>>
{
    private readonly IProtocoloRepository _protocoloRepository;

    public SearchProtocoloQueryHandler(IProtocoloRepository protocoloRepository)
    {
        _protocoloRepository = protocoloRepository;
    }

    public async Task<BaseResult<ProtocolViewModel>> Handle(
        SearchProtocoloQuery request,
        CancellationToken cancellationToken)
    {
        var filter = PredicateBuilder.New<Protocolo>(true);

        if (!string.IsNullOrWhiteSpace(request.NumeroProtocolo))
            filter = filter.And(x => x.NumeroProtocolo == request.NumeroProtocolo);

        if (request.NumeroVia > 0)
            filter = filter.And(x => x.NumeroVia == request.NumeroVia);

        if (!string.IsNullOrWhiteSpace(request.Cpf))
            filter = filter.And(x => x.Cpf == request.Cpf);

        if (!string.IsNullOrWhiteSpace(request.Rg))
            filter = filter.And(x => x.Rg == request.Rg);

        if (!string.IsNullOrWhiteSpace(request.Nome))
            filter = filter.And(x => x.Nome == request.Nome);

        if (!string.IsNullOrWhiteSpace(request.NomeMae))
            filter = filter.And(x => x.NomeMae == request.NomeMae);

        if (!string.IsNullOrWhiteSpace(request.NomePai))
            filter = filter.And(x => x.NomePai == request.NomePai);

        if (request.Id != Guid.Empty)
            filter = filter.And(x => x.Id == request.Id);

        if (request.CreatedAt != default)
            filter = filter.And(x => x.CreatedAt == request.CreatedAt);

        if (request.UpdatedAt != default)
            filter = filter.And(x => x.UpdatedAt == request.UpdatedAt);

        if (request.DeletedAt.HasValue && request.DeletedAt != default)
            filter = filter.And(x => x.DeletedAt == request.DeletedAt);

        var orderMappings = new Dictionary<string, Func<IQueryable<Protocolo>, IOrderedQueryable<Protocolo>>>
        {
            { "Id", x => x.OrderBy(n => n.Id) },
            { "NumeroProtocolo", x => x.OrderBy(n => n.NumeroProtocolo) },
            { "NumeroVia", x => x.OrderBy(n => n.NumeroVia) },
            { "Cpf", x => x.OrderBy(n => n.Cpf) },
            { "Rg", x => x.OrderBy(n => n.Rg) },
            { "Nome", x => x.OrderBy(n => n.Nome) },
            { "NomeMae", x => x.OrderBy(n => n.NomeMae) },
            { "NomePai", x => x.OrderBy(n => n.NomePai) },
            { "CreatedAt", x => x.OrderBy(n => n.CreatedAt) },
            { "UpdatedAt", x => x.OrderBy(n => n.UpdatedAt) },
            { "DeletedAt", x => x.OrderBy(n => n.DeletedAt) }
        };

        var orderKey = string.IsNullOrWhiteSpace(request.Order) ? "Id" : request.Order;

        if (!orderMappings.TryGetValue(orderKey, out var ordeBy))
        {
            ordeBy = x => x.OrderBy(n => n.Id);
        }

        var result = await _protocoloRepository.Search(
            filter,
            ordeBy,
            request.PageSize,
            request.PageIndex);

        return new BaseResult<ProtocolViewModel>(
            result.Data.Select(ProtocolViewModel.FromEntity).ToList(),
            result.PagedResult);
    }
}

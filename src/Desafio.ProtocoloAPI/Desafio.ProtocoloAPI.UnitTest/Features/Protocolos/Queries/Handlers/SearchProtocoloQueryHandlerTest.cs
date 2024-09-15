using Desafio.ProtocoloAPI.Application.Features.Protocolos.Queries;
using Desafio.ProtocoloAPI.Application.Features.Protocolos.Queries.Handlers;
using Desafio.ProtocoloAPI.Core.Repositories;
using Desafio.ProtocoloAPI.UnitTest.Fakes;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Desafio.ProtocoloAPI.UnitTest.Features.Protocolos.Queries.Handlers;

public class SearchProtocoloQueryHandlerTest : BaseTest, IClassFixture<BaseTest>
{
    private readonly ServiceProvider _serviceProvide;

    public SearchProtocoloQueryHandlerTest(BaseTest fixture)
    {
        _serviceProvide = fixture.ServiceProvider;
    }

    [Fact(DisplayName = "Test search Protocolo success")]
    [Trait("SearchProtocoloQueryHandlerTest", "SearchProtocoloQueryHandler Tests")]
    public async Task SearchProtocoloQueryHandlerSuccess()
    {
        // Arrange
        var protocoloRepository = _serviceProvide.GetService<IProtocoloRepository>();
        var faker = FakeProtocolo.GetValid(1).First();

        await protocoloRepository.Add(faker);

        // Act
        var queryHandler = new SearchProtocoloQueryHandler(protocoloRepository);

        var query = new SearchProtocoloQuery
        {
            NumeroProtocolo = faker.NumeroProtocolo,
            NumeroVia = faker.NumeroVia,
            Cpf = faker.Cpf,
            Rg = faker.Rg,
            Nome = faker.Nome,
            NomeMae = faker.NomeMae,
            NomePai = faker.NomePai,
            Id = faker.Id,
            Order = "Id"
        };

        var result = await queryHandler.Handle(query, CancellationToken.None);

        // Assert
        protocoloRepository.Should().NotBeNull();
        result.Should().NotBeNull();
    }

    [Fact(DisplayName = "Test search Protocolo failure")]
    [Trait("SearchProtocoloQueryHandlerTest", "SearchProtocoloQueryHandler Tests")]
    public async Task SearchProtocoloQueryHandlerFailure()
    {
        // Arrange
        var protocoloRepository = _serviceProvide.GetService<IProtocoloRepository>();

        // Act
        var queryHandler = new SearchProtocoloQueryHandler(protocoloRepository);

        var query = new SearchProtocoloQuery
        {
            Id = Guid.NewGuid(),
            Order = "Id"
        };

        var result = await queryHandler.Handle(query, CancellationToken.None);

        // Assert
        protocoloRepository.Should().NotBeNull();
        result.Data.Should().BeEmpty();
    }
}

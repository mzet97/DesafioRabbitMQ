using Desafio.ProtocoloAPI.Application.Dtos.InputModels;
using Desafio.ProtocoloAPI.Application.Features.Protocolos.Subscribers;
using Desafio.ProtocoloAPI.Core.Entities;
using Desafio.ProtocoloAPI.Core.Repositories;
using Desafio.ProtocoloAPI.Infrastructure.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Desafio.ProtocoloAPI.UnitTest.Features.Protocolos.Subscribers
{
    public class ProcessProtocoloTests : BaseTest
    {
        private readonly ProtocoloSubscriber _subscriber;
        private readonly IProtocoloRepository _protocoloRepository;

        public ProcessProtocoloTests()
        {
            _protocoloRepository = ServiceProvider.GetRequiredService<IProtocoloRepository>();

            var messageBusClientMock = new Mock<IMessageBusClient>();
            var loggerMock = new Mock<ILogger<ProtocoloSubscriber>>();

            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeMock.Setup(s => s.ServiceProvider).Returns(ServiceProvider);
            serviceScopeFactoryMock.Setup(f => f.CreateScope()).Returns(serviceScopeMock.Object);

            _subscriber = new ProtocoloSubscriber(
                messageBusClientMock.Object,
                serviceScopeFactoryMock.Object,
                loggerMock.Object
            );
        }

        [Fact(DisplayName = "Deve retornar falso quando o protocolo for nulo")]
        public async Task ProcessProtocolo_DeveRetornarFalso_QuandoProtocoloForNulo()
        {
            // Act
            var result = await _subscriber.ProcessProtocolo(null, _protocoloRepository);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Deve retornar falso quando a validação falhar")]
        public async Task ProcessProtocolo_DeveRetornarFalso_QuandoValidacaoFalhar()
        {
            // Arrange
            var protocoloInvalido = new ProtocoloInput
            {
                NumeroProtocolo = "",
                NumeroVia = 0,
                Cpf = "cpf_invalido",
                Rg = "",
                Nome = "",
                NomeMae = "",
                NomePai = "",
                Foto = ""
            };

            // Act
            var result = await _subscriber.ProcessProtocolo(protocoloInvalido, _protocoloRepository);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Deve retornar falso quando NumeroProtocolo estiver duplicado")]
        public async Task ProcessProtocolo_DeveRetornarFalso_QuandoNumeroProtocoloDuplicado()
        {
            // Arrange
            await LimparBancoDeDados();

            var protocoloInput = CriarProtocoloInputValido();

            var protocoloExistente = new Protocolo(
                protocoloInput.NumeroProtocolo,
                2,
                "12345678901",
                "MG7654321",
                "Nome Existente",
                "Mãe Existente",
                "Pai Existente",
                new byte[] { 1, 2, 3 }
            );
            await _protocoloRepository.Add(protocoloExistente);

            // Act
            var result = await _subscriber.ProcessProtocolo(protocoloInput, _protocoloRepository);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Deve retornar falso quando CPF e NumeroVia estiverem duplicados")]
        public async Task ProcessProtocolo_DeveRetornarFalso_QuandoCpfENumeroViaDuplicados()
        {
            // Arrange
            await LimparBancoDeDados();

            var protocoloInput = CriarProtocoloInputValido();

            var protocoloExistente = new Protocolo(
                "NumeroProtocoloDiferente",
                protocoloInput.NumeroVia,
                protocoloInput.Cpf,
                "RGDiferente",
                "Nome Existente",
                "Mãe Existente",
                "Pai Existente",
                new byte[] { 1, 2, 3 }
            );
            await _protocoloRepository.Add(protocoloExistente);

            // Act
            var result = await _subscriber.ProcessProtocolo(protocoloInput, _protocoloRepository);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Deve retornar falso quando RG e NumeroVia estiverem duplicados")]
        public async Task ProcessProtocolo_DeveRetornarFalso_QuandoRgENumeroViaDuplicados()
        {
            // Arrange
            await LimparBancoDeDados();

            var protocoloInput = CriarProtocoloInputValido();

            var protocoloExistente = new Protocolo(
                "NumeroProtocoloDiferente",
                protocoloInput.NumeroVia,
                "CPFDiferente",
                protocoloInput.Rg,
                "Nome Existente",
                "Mãe Existente",
                "Pai Existente",
                new byte[] { 1, 2, 3 }
            );
            await _protocoloRepository.Add(protocoloExistente);

            // Act
            var result = await _subscriber.ProcessProtocolo(protocoloInput, _protocoloRepository);

            // Assert
            Assert.False(result);
        }

        private ProtocoloInput CriarProtocoloInputValido()
        {
            return new ProtocoloInput
            {
                NumeroProtocolo = "123456",
                NumeroVia = 1,
                Cpf = "45503145089",
                Rg = "MG1234567",
                Nome = "Nome de Teste",
                NomeMae = "Nome da Mãe",
                NomePai = "Nome do Pai",
                Foto = Convert.ToBase64String(new byte[] { 1, 2, 3, 4 })
            };
        }

        private async Task LimparBancoDeDados()
        {
            await _protocoloRepository.RemoveAll();
        }
    }
}

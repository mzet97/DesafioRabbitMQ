using Desafio.ProtocoloAPI.Application.Dtos.InputModels;
using Desafio.ProtocoloAPI.Application.Features.Protocolos.Subscribers;
using Desafio.ProtocoloAPI.Core.Entities;
using Desafio.ProtocoloAPI.Core.Repositories;
using Desafio.ProtocoloAPI.Infrastructure.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace Desafio.ProtocoloAPI.UnitTest.Features.Protocolos.Subscribers
{
    public class ProtocoloSubscriberTests : BaseTest
    {
        [Fact(DisplayName = "Deve processar a mensagem e publicar o protocolo aprovado")]
        public async Task OnMessageReceived_ShouldProcessMessageAndPublishProtocoloApproved()
        {
            // Arrange
            var messageBusClient = new Mock<IMessageBusClient>();
            var logger = new Mock<ILogger<ProtocoloSubscriber>>();

            var protocoloRepository = ServiceProvider.GetRequiredService<IProtocoloRepository>();

            // Clear the repository
            var existingProtocols = await protocoloRepository.GetAll();
            foreach (var protocolo in existingProtocols)
            {
                await protocoloRepository.Remove(protocolo.Id);
            }

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            var serviceScope = new Mock<IServiceScope>();
            serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);
            serviceScope.Setup(x => x.ServiceProvider).Returns(ServiceProvider);

            var subscriber = new ProtocoloSubscriber(
                messageBusClient.Object,
                serviceScopeFactory.Object,
                logger.Object
            );

            var protocoloInput = new ProtocoloInput
            {
                NumeroProtocolo = "123",
                NumeroVia = 1,
                Cpf = "45503145089",
                Rg = "123456789",
                Nome = "Test Name",
                NomeMae = "Test Mother",
                NomePai = "Test Father",
                Foto = Convert.ToBase64String(new byte[] { 1, 2, 3 })
            };

            var message = JsonSerializer.Serialize(protocoloInput);

            // Act
            subscriber.OnMessageReceived(message);

        }



        [Fact(DisplayName = "Deve processar a mensagem e publicar o protocolo recusado")]
        public async Task OnMessageReceived_ShouldProcessMessageAndPublishProtocoloRefused()
        {
            // Arrange
            var messageBusClient = new Mock<IMessageBusClient>();
            var logger = new Mock<ILogger<ProtocoloSubscriber>>();

            var protocoloRepository = ServiceProvider.GetRequiredService<IProtocoloRepository>();

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            var serviceScope = new Mock<IServiceScope>();
            serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);
            serviceScope.Setup(x => x.ServiceProvider).Returns(ServiceProvider);

            var subscriber = new ProtocoloSubscriber(
                messageBusClient.Object,
                serviceScopeFactory.Object,
                logger.Object
            );

            var protocoloInput = new ProtocoloInput
            {
                NumeroProtocolo = "123",
                NumeroVia = 1,
                Cpf = "12345678901",
                Rg = "123456789",
                Nome = "Test Name",
                NomeMae = "Test Mother",
                NomePai = "Test Father"
            };

            var message = JsonSerializer.Serialize(protocoloInput);

            await protocoloRepository.Add(new Protocolo("123", 1, "12345678901", "123456789", "Test Name", "Test Mother", "Test Father", new byte[0]));

            var taskCompletionSource = new TaskCompletionSource<bool>();

            messageBusClient
                .Setup(x => x.Publish(It.IsAny<ProtocoloInput>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() =>
                {
                    taskCompletionSource.SetResult(true);
                });

            // Act
            subscriber.OnMessageReceived(message);

            await taskCompletionSource.Task;

            // Assert
            messageBusClient.Verify(x => x.Publish(
                It.Is<ProtocoloInput>(p => p.NumeroProtocolo == "123"),
                "protocolos.refused",
                "protocolos",
                "protocolos_finish_queue"), Times.Once);

            logger.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Protocolo 123 recusado e enviado para a fila protocolos.refused")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.AtLeastOnce);
        }
    }
}

﻿using Desafio.ProtocoloPublisher.Core.Fakes;

namespace Desafio.ProtocoloPublisher.Core.Test.Fakes;

public class FakeProtocoloTests
{
    [Fact(DisplayName = "Test get valid FakeProtocolo")]
    [Trait("FakeProtocoloTests", "FakeProtocolo Unit Tests")]
    public void TestGetValid()
    {
        // Arrange
        int quantity = 10;

        // Act
        var protocolos = FakeProtocolo.GetValid(quantity);

        // Assert
        Assert.Equal(quantity, protocolos.Count());
        Assert.All(protocolos, protocolo =>
        {
            Assert.False(string.IsNullOrWhiteSpace(protocolo.NumeroProtocolo));
            Assert.True(protocolo.NumeroVia >= 1);
            Assert.False(string.IsNullOrWhiteSpace(protocolo.Cpf));
            Assert.False(string.IsNullOrWhiteSpace(protocolo.Rg));
            Assert.False(string.IsNullOrWhiteSpace(protocolo.Nome));
            Assert.NotNull(protocolo.Foto);
            Assert.NotEmpty(protocolo.Foto);
        });
    }

    [Fact(DisplayName = "Test get invalid FakeProtocolo")]
    [Trait("FakeProtocoloTests", "FakeProtocolo Unit Tests")]
    public void TestGetInvalid()
    {
        // Arrange
        int quantity = 10;

        // Act
        var protocolos = FakeProtocolo.GetInvalid(quantity);

        // Assert
        Assert.Equal(quantity, protocolos.Count());
        Assert.Contains(protocolos, p => p.Cpf == "12345678901");
        Assert.Contains(protocolos, p => p.NumeroProtocolo == "DUPLICATE-PROTOCOLO");
        Assert.Contains(protocolos, p => string.IsNullOrWhiteSpace(p.Nome));
    }

    [Fact(DisplayName = "Test get random FakeProtocolo")]
    [Trait("FakeProtocoloTests", "FakeProtocolo Unit Tests")]
    public void TestGetRandom()
    {
        // Arrange

        // Act
        var protocolos = FakeProtocolo.GetRandom();

        // Assert
        Assert.Equal(10, protocolos.Count());
        int validCount = protocolos.Count(p => !string.IsNullOrWhiteSpace(p.Nome) && p.NumeroProtocolo != "DUPLICATE-PROTOCOLO" && p.Cpf != "12345678901");
        int invalidCount = protocolos.Count() - validCount;

        Assert.Equal(6, validCount);
        Assert.Equal(4, invalidCount);
    }
}
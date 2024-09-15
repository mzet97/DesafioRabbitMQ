using Bogus;
using Desafio.ProtocoloAPI.Application.Features.Auth.Commands;
using Desafio.ProtocoloAPI.Application.Features.Auth.ViewModels;
using Desafio.ProtocoloAPI.Application.Features.Protocolos.ViewModels;
using Desafio.ProtocoloAPI.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;

namespace Desafio.ProtocoloAPI.IntegrationTest;

public class ProtocoloControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly string hostApi = "https://localhost:8081/api/";
    private readonly Faker _faker;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ProtocoloControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _faker = new Faker("pt_BR");
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
        });
    }

    [Fact(DisplayName = "Get Protocolo with success")]
    [Trait("ProtocoloControllerTest", "Protocolo Controller Tests")]
    public async Task GetProtocoloWithSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Primeiro, registre um usuário e obtenha o token de autenticação
        var registerUserViewModel = new RegisterUserCommand
        {
            Email = _faker.Internet.Email(),
            Password = "Admin@123"
        };

        var responseRegister = await client.PostAsync($"{hostApi}auth/register",
            new StringContent(JsonSerializer.Serialize(registerUserViewModel), System.Text.Encoding.UTF8, "application/json"));
        var jsonRegister = await responseRegister.Content.ReadAsStringAsync();
        var dataRegister = JsonSerializer.Deserialize<LoginResponseViewModel>(jsonRegister, _jsonSerializerOptions);

        Assert.Equal(HttpStatusCode.OK, responseRegister.StatusCode);
        Assert.NotNull(dataRegister?.AccessToken);

        // Configure o token de autenticação no cabeçalho da requisição
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dataRegister.AccessToken);

        // Crie um protocolo de teste ou certifique-se de que haja dados no banco para serem consultados
        // Aqui, assumiremos que já existe um protocolo com o NúmeroProtocolo "123456"
        var queryParameters = new Dictionary<string, string>
        {
            { "PageIndex", "1" },
            { "PageSize", "10" }
        };

        var queryString = new FormUrlEncodedContent(queryParameters).ReadAsStringAsync().Result;

        // Act
        var response = await client.GetAsync($"{hostApi}protocolo?{queryString}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<BaseResult<ProtocolViewModel>>(jsonResponse, _jsonSerializerOptions);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.PagedResult);
        Assert.Equal(result.PagedResult.CurrentPage, 1);
        Assert.Equal(result.PagedResult.PageSize, 10);
    }

    [Fact(DisplayName = "Get Protocolo without authentication")]
    [Trait("ProtocoloControllerTest", "Protocolo Controller Tests")]
    public async Task GetProtocoloWithoutAuthentication()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Não configurar o token de autenticação
        var queryParameters = new Dictionary<string, string>
        {
            { "PageIndex", "1" },
            { "PageSize", "10" }
        };

        var queryString = new FormUrlEncodedContent(queryParameters).ReadAsStringAsync().Result;

        // Act
        var response = await client.GetAsync($"{hostApi}protocolo?{queryString}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

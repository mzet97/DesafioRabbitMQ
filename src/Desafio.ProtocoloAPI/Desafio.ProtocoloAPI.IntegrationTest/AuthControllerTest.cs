using Bogus;
using Desafio.ProtocoloAPI.Application.Features.Auth.Commands;
using Desafio.ProtocoloAPI.Application.Features.Auth.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;

namespace Desafio.ProtocoloAPI.IntegrationTest;

public class AuthControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    private readonly string hostApi = "https://localhost:8081/api/";

    private readonly Faker _faker;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public AuthControllerTest(WebApplicationFactory<Program> factory)
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

    [Fact(DisplayName = "Register user with success")]
    [Trait("AuthControllerTest", "Auth Controller Tests")]
    public async Task RegisterWithSuccess()
    {
        // Arrange
        var viewModel = new RegisterUserCommand();
        viewModel.Email = _faker.Internet.Email();
        viewModel.Password = "Admin@123";

        // Act
        var client = _factory.CreateClient();
        var response = await client.PostAsync($"{hostApi}auth/register",
            new StringContent(JsonSerializer.Serialize(viewModel), System.Text.Encoding.UTF8, "application/json"));
        string json = await response.Content.ReadAsStringAsync();
        var userData = JsonSerializer.Deserialize<LoginResponseViewModel>(json, _jsonSerializerOptions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(userData);
        Assert.NotNull(userData?.UserToken);
        Assert.Equal(viewModel.Email, userData?.UserToken.Email);
        Assert.True(userData?.ExpiresIn > 0);
        Assert.True(userData?.AccessToken.Length > 0);
        Assert.True(userData?.UserToken.Id.Length > 0);
        Assert.True(userData?.UserToken.Claims.ToList().Count() > 0);
    }

    [Fact(DisplayName = "Login user with error")]
    [Trait("AuthControllerTest", "Auth Controller Tests")]
    public async Task LoginWithError()
    {
        // Arrange
        var viewModel = new LoginUserCommand();
        viewModel.Email = _faker.Internet.Email();
        viewModel.Password = "Admin@123";

        // Act
        var client = _factory.CreateClient();
        var response = await client.PostAsync($"{hostApi}auth/login",
            new StringContent(JsonSerializer.Serialize(viewModel), System.Text.Encoding.UTF8, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName = "Login user with success")]
    [Trait("AuthControllerTest", "Auth Controller Tests")]
    public async Task LoginWithSuccess()
    {
        // Arrange
        var registerUserViewModel = new RegisterUserCommand();
        registerUserViewModel.Email = _faker.Internet.Email();
        registerUserViewModel.Password = "Admin@123";

        var client = _factory.CreateClient();
        var responseRegister = await client.PostAsync($"{hostApi}auth/register",
            new StringContent(JsonSerializer.Serialize(registerUserViewModel), System.Text.Encoding.UTF8, "application/json"));
        string jsonRegister = await responseRegister.Content.ReadAsStringAsync();
        var dataRegiter = JsonSerializer.Deserialize<LoginResponseViewModel>(jsonRegister, _jsonSerializerOptions);

        var loginUserViewModel = new LoginUserCommand();
        loginUserViewModel.Email = registerUserViewModel.Email;
        loginUserViewModel.Password = "Admin@123";

        // Act
        var responseLogin = await client.PostAsync($"{hostApi}auth/login",
            new StringContent(JsonSerializer.Serialize(loginUserViewModel), System.Text.Encoding.UTF8, "application/json"));
        string jsonLogin = await responseLogin.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<LoginResponseViewModel>(jsonLogin, _jsonSerializerOptions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, responseRegister.StatusCode);
        Assert.Equal(HttpStatusCode.OK, responseLogin.StatusCode);
        Assert.NotNull(dataRegiter);
        Assert.NotNull(data);
        Assert.NotNull(data?.UserToken);
        Assert.NotNull(dataRegiter?.UserToken);
        Assert.True(data?.ExpiresIn > 0);
        Assert.True(data?.AccessToken.Length > 0);
        Assert.True(data?.UserToken.Id.Length > 0);
        Assert.True(data?.UserToken.Claims.ToList().Count() > 0);
        Assert.True(dataRegiter?.ExpiresIn > 0);
        Assert.True(dataRegiter?.AccessToken.Length > 0);
        Assert.True(dataRegiter?.UserToken.Id.Length > 0);
        Assert.True(dataRegiter?.UserToken.Claims.ToList().Count() > 0);
    }
}

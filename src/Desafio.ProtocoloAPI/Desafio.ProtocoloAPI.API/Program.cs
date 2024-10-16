using Desafio.ProtocoloAPI.API.Configuration;
using Desafio.ProtocoloAPI.API.Extensions;
using Desafio.ProtocoloAPI.Application.Configuration;
using Desafio.ProtocoloAPI.Application.Features.Protocolos.Subscribers;
using Desafio.ProtocoloAPI.Infrastructure.Configuration;
using Desafio.ProtocoloAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables();

    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "localhost:6379";
        options.InstanceName = "RedisCacheInstance";
    });

    builder.Services.AddOutputCache(options =>
    {
        options.AddBasePolicy(builder =>
            builder.Expire(TimeSpan.FromSeconds(10)));
    });

    builder.Services.Configure<KestrelServerOptions>(options =>
    {
        options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(2);
        options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
    });

    builder.Services.Configure<KestrelServerOptions>(options =>
    {
        options.Limits.MaxConcurrentConnections = 100000;  // Aumentar para suportar mais conexões simultâneas
        options.Limits.MaxConcurrentUpgradedConnections = 100000; // Útil se estiver usando WebSockets
    });

    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.Limits.MaxRequestBufferSize = 1073741824; // 1 GB
        serverOptions.Limits.MaxResponseBufferSize = 1073741824; // 1 GB
    });

    builder.Services.AddOpenTelemetryConfig();
    builder.Logging.AddOpenTelemetryConfig();

    builder.Services.AddIdentityConfig(builder.Configuration);
    builder.Services.AddCorsConfig();
    builder.Services.ResolveDependenciesInfrastructure();
    builder.Services.AddMessageBus(builder.Configuration);
    builder.Services.AddApplicationServices();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerConfig();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHostedService<ProtocoloSubscriber>();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DataIdentityDbContext>();
        dbContext.Database.Migrate();
    }

    app.UseOutputCache();
    app.UseSwagger();
    app.UseSwaggerUI();

    if (app.Environment.IsDevelopment())
    {
        app.UseCors("Development");

    }
    else
    {
        app.UseCors("Production");
        app.UseHsts();
    }

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseExceptionHandler(options => { });
    app.UseRouting();

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseStaticFiles();
    app.MapControllers();
    app.UseSwaggerConfig();


    app.Run();
}
catch(Exception ex)
{
    Console.WriteLine("Erro ao roda a aplicacao");
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.InnerException?.Message);
}

public partial class Program { }
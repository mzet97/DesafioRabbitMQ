using Desafio.ProtocoloAPI.Infrastructure.Configuration;
using Desafio.ProtocoloAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Desafio.ProtocoloAPI.UnitTest;

public class BaseTest : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }

    public BaseTest()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<DataIdentityDbContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryDb");
        }, ServiceLifetime.Scoped);

        serviceCollection.ResolveDependenciesInfrastructure();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServiceProvider?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

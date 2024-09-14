using Desafio.ProtocoloAPI.Core.Repositories;
using Desafio.ProtocoloAPI.Infrastructure.Persistence;
using Desafio.ProtocoloAPI.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Desafio.ProtocoloAPI.Infrastructure.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection ResolveDependenciesInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<DbContext, DataIdentityDbContext>();

        #region Repository

        services.AddScoped<IProtocoloRepository, ProtocoloRepository>();

        #endregion Repository

        return services;
    }
}
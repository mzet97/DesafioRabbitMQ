using Desafio.ProtocoloAPI.Application.Common.Behaviours;
using Desafio.ProtocoloAPI.Application.Dtos.InputModels;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
namespace Desafio.ProtocoloAPI.Application.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddValidatorsFromAssembly(typeof(BaseSearch).Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(BaseSearch).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        return services;
    }
}

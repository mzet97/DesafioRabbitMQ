using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Desafio.ProtocoloAPI.API.Configuration;

public static class OpenTelemetryConfig
{
    public static IServiceCollection AddOpenTelemetryConfig(this IServiceCollection services)
    {
       services.AddOpenTelemetry()
         .WithTracing(traceBuilder =>
         {
             traceBuilder
                 .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyAspNetCoreApi"))
                 .AddAspNetCoreInstrumentation()
                 .AddHttpClientInstrumentation()
                 .AddOtlpExporter(options =>
                 {
                     options.Endpoint = new Uri("http://otel-collector:4317");
                 });
         });

        return services;
    }

    public static ILoggingBuilder AddOpenTelemetryConfig(this ILoggingBuilder logging)
    {
        logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyAspNetCoreApi"));
            options.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri("http://otel-collector:4317");
            });
        });

        return logging;
    }
}

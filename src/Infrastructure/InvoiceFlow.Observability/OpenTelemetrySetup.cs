using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace InvoiceFlow.Observability;

public static class OpenTelemetrySetup
{
    public static IServiceCollection AddInvoiceFlowTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithTracing(t => t.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation())
            .WithMetrics(m => m.AddAspNetCoreInstrumentation());
        return services;
    }
}

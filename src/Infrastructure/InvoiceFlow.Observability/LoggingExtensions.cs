using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace InvoiceFlow.Observability;

public static class LoggingExtensions
{
    public static WebApplicationBuilder AddInvoiceFlowLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration).WriteTo.Console());
        return builder;
    }
}

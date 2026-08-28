using InvoiceFlow.Application.Abstractions;
using InvoiceFlow.Infrastructure.Caching;
using InvoiceFlow.Infrastructure.Identity;
using InvoiceFlow.Infrastructure.Persistence;
using InvoiceFlow.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var postgres = configuration.GetConnectionString("Postgres")
                       ?? "Host=localhost;Port=5432;Database=invoiceflow;Username=invoiceflow;Password=invoiceflow";

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(postgres));
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        var redis = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        services.AddStackExchangeRedisCache(options => options.Configuration = redis);
        services.AddScoped<ICacheService, RedisCacheService>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IEmailService, SmtpEmailService>();
        services.AddSingleton<AzureBlobStorageService>();

        return services;
    }
}

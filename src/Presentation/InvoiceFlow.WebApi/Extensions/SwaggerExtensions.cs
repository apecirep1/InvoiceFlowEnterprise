namespace InvoiceFlow.WebApi.Extensions;
public static class SwaggerExtensions
{
    public static IServiceCollection AddInvoiceFlowSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}

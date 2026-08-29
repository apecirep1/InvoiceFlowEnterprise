using Testcontainers.PostgreSql;

namespace InvoiceFlow.IntegrationTests;

public sealed class SharedContainerFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; } = new PostgreSqlBuilder()
        .WithImage("postgres:17-alpine")
        .WithDatabase("invoiceflow")
        .WithUsername("invoiceflow")
        .WithPassword("invoiceflow")
        .Build();

    public Task InitializeAsync() => Container.StartAsync();
    public Task DisposeAsync() => Container.DisposeAsync().AsTask();
}

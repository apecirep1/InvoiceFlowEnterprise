using InvoiceFlow.Application.Abstractions;
using InvoiceFlow.Domain.Invoices;
using InvoiceFlow.Domain.Vendors;
using InvoiceFlow.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

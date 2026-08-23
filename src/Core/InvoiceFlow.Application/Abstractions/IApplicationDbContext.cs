using InvoiceFlow.Domain.Invoices;
using InvoiceFlow.Domain.Vendors;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Invoice> Invoices { get; }
    DbSet<Vendor> Vendors { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

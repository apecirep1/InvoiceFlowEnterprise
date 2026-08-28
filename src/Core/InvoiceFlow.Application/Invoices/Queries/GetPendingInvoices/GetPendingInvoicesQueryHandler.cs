using InvoiceFlow.Application.Abstractions;
using InvoiceFlow.Domain.Invoices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.Application.Invoices.Queries.GetPendingInvoices;

public sealed class GetPendingInvoicesQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetPendingInvoicesQuery, IReadOnlyCollection<PendingInvoiceDto>>
{
    public async Task<IReadOnlyCollection<PendingInvoiceDto>> Handle(GetPendingInvoicesQuery request, CancellationToken cancellationToken) =>
        await db.Invoices.AsNoTracking()
            .Where(x => x.Status == InvoiceStatus.PendingReview || x.Status == InvoiceStatus.PendingExtraction)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new PendingInvoiceDto(x.Id, x.InvoiceNumber, x.VendorName, x.Total.Amount, x.Total.Currency, x.Status.ToString()))
            .ToListAsync(cancellationToken);
}

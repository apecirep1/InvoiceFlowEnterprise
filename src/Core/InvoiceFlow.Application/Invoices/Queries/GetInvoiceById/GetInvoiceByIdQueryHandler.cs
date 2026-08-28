using InvoiceFlow.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.Application.Invoices.Queries.GetInvoiceById;

public sealed class GetInvoiceByIdQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetInvoiceByIdQuery, InvoiceDetailsDto?>
{
    public Task<InvoiceDetailsDto?> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken) =>
        db.Invoices.AsNoTracking()
          .Where(x => x.Id == request.InvoiceId)
          .Select(x => new InvoiceDetailsDto(x.Id, x.InvoiceNumber, x.VendorName, x.Total.Amount, x.Total.Currency, x.Status.ToString(), x.ExtractionConfidence == null ? null : x.ExtractionConfidence.Value))
          .FirstOrDefaultAsync(cancellationToken);
}

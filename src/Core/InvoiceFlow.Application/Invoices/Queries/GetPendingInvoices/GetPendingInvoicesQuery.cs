using MediatR;
namespace InvoiceFlow.Application.Invoices.Queries.GetPendingInvoices;
public sealed record GetPendingInvoicesQuery : IRequest<IReadOnlyCollection<PendingInvoiceDto>>;

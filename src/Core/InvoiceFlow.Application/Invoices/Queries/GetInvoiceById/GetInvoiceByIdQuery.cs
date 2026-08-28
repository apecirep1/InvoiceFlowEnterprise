using MediatR;
namespace InvoiceFlow.Application.Invoices.Queries.GetInvoiceById;
public sealed record GetInvoiceByIdQuery(Guid InvoiceId) : IRequest<InvoiceDetailsDto?>;

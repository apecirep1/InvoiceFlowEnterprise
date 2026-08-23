using MediatR;
namespace InvoiceFlow.Application.Invoices.Commands.RejectInvoice;
public sealed record RejectInvoiceCommand(Guid InvoiceId, string Reason) : IRequest;

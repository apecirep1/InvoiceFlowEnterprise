using MediatR;
namespace InvoiceFlow.Application.Invoices.Commands.ProcessInvoicePdf;
public sealed record ProcessInvoicePdfCommand(Stream Document, string FileName) : IRequest<Guid>;

using MediatR;
namespace InvoiceFlow.Application.Invoices.Commands.ApproveInvoice;
public sealed record ApproveInvoiceCommand(Guid InvoiceId) : IRequest;

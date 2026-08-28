namespace InvoiceFlow.Application.Invoices.Queries.GetPendingInvoices;
public sealed record PendingInvoiceDto(Guid Id, string InvoiceNumber, string VendorName, decimal Total, string Currency, string Status);

namespace InvoiceFlow.Application.Invoices.Queries.GetInvoiceById;
public sealed record InvoiceDetailsDto(Guid Id, string InvoiceNumber, string VendorName, decimal Total, string Currency, string Status, decimal? Confidence);

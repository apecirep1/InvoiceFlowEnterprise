using InvoiceFlow.Domain.Invoices;

namespace InvoiceFlow.Application.Common.Mappings;

public static class InvoiceMappingConfig
{
    public static string ToSummary(Invoice invoice) =>
        $"{invoice.InvoiceNumber} | {invoice.VendorName} | {invoice.Total} | {invoice.Status}";
}

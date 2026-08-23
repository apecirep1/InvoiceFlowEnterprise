namespace InvoiceFlow.Application.Abstractions.AI;

public sealed record ExtractedInvoiceData(
    string InvoiceNumber,
    string VendorName,
    decimal Total,
    string Currency,
    decimal Confidence);

public interface IDocumentExtractor
{
    Task<ExtractedInvoiceData> ExtractAsync(Stream document, string fileName, CancellationToken cancellationToken);
}

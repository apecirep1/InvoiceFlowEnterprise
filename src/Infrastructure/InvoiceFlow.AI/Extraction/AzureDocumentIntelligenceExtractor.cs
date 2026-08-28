using InvoiceFlow.Application.Abstractions.AI;

namespace InvoiceFlow.AI.Extraction;

public sealed class AzureDocumentIntelligenceExtractor(OpenAiVisionExtractor fallback) : IDocumentExtractor
{
    public Task<ExtractedInvoiceData> ExtractAsync(Stream document, string fileName, CancellationToken cancellationToken)
        => fallback.ExtractAsync(document, fileName, cancellationToken);
}

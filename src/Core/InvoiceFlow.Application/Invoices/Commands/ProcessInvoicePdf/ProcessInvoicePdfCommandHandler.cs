using InvoiceFlow.Application.Abstractions;
using InvoiceFlow.Application.Abstractions.AI;
using InvoiceFlow.Domain.Invoices;
using InvoiceFlow.Domain.Invoices.ValueObjects;
using MediatR;

namespace InvoiceFlow.Application.Invoices.Commands.ProcessInvoicePdf;

public sealed class ProcessInvoicePdfCommandHandler(
    IApplicationDbContext db,
    IDocumentExtractor extractor,
    IFraudDetectionModel fraudModel,
    IEmbeddingService embeddings,
    IVectorStore vectorStore)
    : IRequestHandler<ProcessInvoicePdfCommand, Guid>
{
    public async Task<Guid> Handle(ProcessInvoicePdfCommand request, CancellationToken cancellationToken)
    {
        var extracted = await extractor.ExtractAsync(request.Document, request.FileName, cancellationToken);

        var invoice = new Invoice(
            extracted.InvoiceNumber,
            extracted.VendorName,
            Money.Of(extracted.Total, extracted.Currency),
            request.FileName);

        invoice.ApplyExtraction(ConfidenceScore.From(extracted.Confidence));
        db.Invoices.Add(invoice);
        await db.SaveChangesAsync(cancellationToken);

        _ = await fraudModel.AssessAsync(invoice, cancellationToken);
        var text = $"{invoice.InvoiceNumber} {invoice.VendorName} {invoice.Total.Amount} {invoice.Total.Currency}";
        var vector = await embeddings.EmbedAsync(text, cancellationToken);
        await vectorStore.IndexAsync(invoice.Id, text, vector, cancellationToken);

        return invoice.Id;
    }
}

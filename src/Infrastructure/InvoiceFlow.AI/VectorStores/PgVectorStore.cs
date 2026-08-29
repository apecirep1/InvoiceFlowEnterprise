using InvoiceFlow.Application.Abstractions.AI;
namespace InvoiceFlow.AI.VectorStores;
public sealed class PgVectorStore(QdrantVectorStore fallback) : IVectorStore
{
    public Task IndexAsync(Guid invoiceId, string text, float[] embedding, CancellationToken cancellationToken)
        => fallback.IndexAsync(invoiceId, text, embedding, cancellationToken);
    public Task<IReadOnlyCollection<VectorSearchHit>> SearchAsync(float[] embedding, int limit, CancellationToken cancellationToken)
        => fallback.SearchAsync(embedding, limit, cancellationToken);
}

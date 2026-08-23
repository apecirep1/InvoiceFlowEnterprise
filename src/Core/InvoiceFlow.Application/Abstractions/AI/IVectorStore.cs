namespace InvoiceFlow.Application.Abstractions.AI;

public sealed record VectorSearchHit(Guid InvoiceId, string Text, float Score);

public interface IVectorStore
{
    Task IndexAsync(Guid invoiceId, string text, float[] embedding, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<VectorSearchHit>> SearchAsync(float[] embedding, int limit, CancellationToken cancellationToken);
}

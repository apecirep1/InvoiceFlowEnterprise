namespace InvoiceFlow.Application.Abstractions.AI;
public interface IEmbeddingService
{
    Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken);
}

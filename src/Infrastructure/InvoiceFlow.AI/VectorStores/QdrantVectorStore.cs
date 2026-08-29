using System.Collections.Concurrent;
using InvoiceFlow.Application.Abstractions.AI;

namespace InvoiceFlow.AI.VectorStores;

/// <summary>
/// Local in-memory vector store fallback that keeps the demo runnable without Qdrant.
/// Replace registration with a remote Qdrant implementation for production.
/// </summary>
public sealed class QdrantVectorStore : IVectorStore
{
    private static readonly ConcurrentDictionary<Guid, (string Text, float[] Vector)> Items = new();

    public Task IndexAsync(Guid invoiceId, string text, float[] embedding, CancellationToken cancellationToken)
    {
        Items[invoiceId] = (text, embedding);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<VectorSearchHit>> SearchAsync(float[] embedding, int limit, CancellationToken cancellationToken)
    {
        var hits = Items.Select(pair =>
        {
            var score = Cosine(pair.Value.Vector, embedding);
            return new VectorSearchHit(pair.Key, pair.Value.Text, score);
        })
        .OrderByDescending(x => x.Score)
        .Take(limit)
        .ToArray();

        return Task.FromResult<IReadOnlyCollection<VectorSearchHit>>(hits);
    }

    private static float Cosine(float[] a, float[] b)
    {
        var length = Math.Min(a.Length, b.Length);
        float dot = 0, a2 = 0, b2 = 0;
        for (var i = 0; i < length; i++)
        {
            dot += a[i] * b[i];
            a2 += a[i] * a[i];
            b2 += b[i] * b[i];
        }
        var denominator = MathF.Sqrt(a2) * MathF.Sqrt(b2);
        return denominator == 0 ? 0 : dot / denominator;
    }
}

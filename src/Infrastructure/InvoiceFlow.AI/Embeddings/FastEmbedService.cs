using System.Security.Cryptography;
using System.Text;
using InvoiceFlow.Application.Abstractions.AI;

namespace InvoiceFlow.AI.Embeddings;

public sealed class FastEmbedService : IEmbeddingService
{
    public Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken)
    {
        const int dimensions = 384;
        var vector = new float[dimensions];
        var tokens = text.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (var token in tokens)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            var index = BitConverter.ToUInt16(bytes, 0) % dimensions;
            vector[index] += 1f;
        }

        var norm = MathF.Sqrt(vector.Sum(v => v * v));
        if (norm > 0)
            for (var i = 0; i < vector.Length; i++) vector[i] /= norm;

        return Task.FromResult(vector);
    }
}

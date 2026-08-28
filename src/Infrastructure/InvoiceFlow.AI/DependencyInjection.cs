using InvoiceFlow.AI.AnomalyDetection;
using InvoiceFlow.AI.Embeddings;
using InvoiceFlow.AI.Extraction;
using InvoiceFlow.AI.VectorStores;
using InvoiceFlow.Application.Abstractions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceFlow.AI;

public static class DependencyInjection
{
    public static IServiceCollection AddInvoiceFlowAi(this IServiceCollection services)
    {
        services.AddSingleton<OpenAiVisionExtractor>();
        services.AddSingleton<IDocumentExtractor>(sp => sp.GetRequiredService<OpenAiVisionExtractor>());
        services.AddSingleton<IEmbeddingService, FastEmbedService>();
        services.AddSingleton<IFraudDetectionModel, OnnxFraudDetector>();
        services.AddSingleton<QdrantVectorStore>();
        services.AddSingleton<IVectorStore>(sp => sp.GetRequiredService<QdrantVectorStore>());
        return services;
    }
}

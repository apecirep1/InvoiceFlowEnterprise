using InvoiceFlow.Application.Abstractions.AI;
using MediatR;

namespace InvoiceFlow.Application.Invoices.Queries.SemanticSearch;

public sealed class SemanticSearchQueryHandler(IEmbeddingService embeddings, IVectorStore vectorStore)
    : IRequestHandler<SemanticSearchQuery, IReadOnlyCollection<SemanticSearchResultDto>>
{
    public async Task<IReadOnlyCollection<SemanticSearchResultDto>> Handle(SemanticSearchQuery request, CancellationToken cancellationToken)
    {
        var vector = await embeddings.EmbedAsync(request.Text, cancellationToken);
        var hits = await vectorStore.SearchAsync(vector, request.Limit, cancellationToken);
        return hits.Select(x => new SemanticSearchResultDto(x.InvoiceId, x.Text, x.Score)).ToArray();
    }
}

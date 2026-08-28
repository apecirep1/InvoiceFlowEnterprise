using MediatR;
namespace InvoiceFlow.Application.Invoices.Queries.SemanticSearch;
public sealed record SemanticSearchQuery(string Text, int Limit = 10) : IRequest<IReadOnlyCollection<SemanticSearchResultDto>>;

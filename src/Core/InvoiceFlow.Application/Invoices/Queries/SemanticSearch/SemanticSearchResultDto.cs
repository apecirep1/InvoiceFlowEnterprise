namespace InvoiceFlow.Application.Invoices.Queries.SemanticSearch;
public sealed record SemanticSearchResultDto(Guid InvoiceId, string Text, float Score);

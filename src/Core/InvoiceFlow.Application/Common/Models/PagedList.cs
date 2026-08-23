namespace InvoiceFlow.Application.Common.Models;
public sealed record PagedList<T>(IReadOnlyCollection<T> Items, int Page, int PageSize, int TotalCount);

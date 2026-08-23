using InvoiceFlow.Domain.Common;

namespace InvoiceFlow.Domain.Invoices.Events;

public sealed record InvoiceExtractedViaAiEvent(Guid InvoiceId, decimal Confidence, DateTime OccurredOnUtc) : IDomainEvent;

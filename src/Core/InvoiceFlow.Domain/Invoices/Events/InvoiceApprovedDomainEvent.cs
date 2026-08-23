using InvoiceFlow.Domain.Common;

namespace InvoiceFlow.Domain.Invoices.Events;

public sealed record InvoiceApprovedDomainEvent(Guid InvoiceId, DateTime OccurredOnUtc) : IDomainEvent;

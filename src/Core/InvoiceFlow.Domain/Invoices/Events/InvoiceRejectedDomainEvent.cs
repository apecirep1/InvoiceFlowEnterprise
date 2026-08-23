using InvoiceFlow.Domain.Common;

namespace InvoiceFlow.Domain.Invoices.Events;

public sealed record InvoiceRejectedDomainEvent(Guid InvoiceId, string Reason, DateTime OccurredOnUtc) : IDomainEvent;

namespace InvoiceFlow.Domain.Exceptions;

public sealed class InvoiceNotFoundException(Guid id)
    : DomainException($"Invoice '{id}' was not found.");

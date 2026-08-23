using InvoiceFlow.Domain.Common;
using InvoiceFlow.Domain.Exceptions;
using InvoiceFlow.Domain.Invoices.Events;
using InvoiceFlow.Domain.Invoices.ValueObjects;

namespace InvoiceFlow.Domain.Invoices;

public sealed class Invoice : AggregateRoot
{
    private readonly List<InvoiceLineItem> _lineItems = [];

    private Invoice() { }

    public Invoice(string invoiceNumber, string vendorName, Money total, string sourceFileName)
    {
        if (string.IsNullOrWhiteSpace(invoiceNumber)) throw new DomainException("Invoice number is required.");
        if (string.IsNullOrWhiteSpace(vendorName)) throw new DomainException("Vendor name is required.");

        InvoiceNumber = invoiceNumber;
        VendorName = vendorName;
        Total = total;
        SourceFileName = sourceFileName;
        Status = InvoiceStatus.PendingExtraction;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public string InvoiceNumber { get; private set; } = string.Empty;
    public string VendorName { get; private set; } = string.Empty;
    public Money Total { get; private set; } = Money.Of(0);
    public string SourceFileName { get; private set; } = string.Empty;
    public InvoiceStatus Status { get; private set; }
    public ConfidenceScore? ExtractionConfidence { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public string? RejectionReason { get; private set; }
    public IReadOnlyCollection<InvoiceLineItem> LineItems => _lineItems.AsReadOnly();

    public void ApplyExtraction(ConfidenceScore confidence, IEnumerable<InvoiceLineItem>? items = null)
    {
        ExtractionConfidence = confidence;
        if (items is not null)
        {
            _lineItems.Clear();
            _lineItems.AddRange(items);
        }

        Status = confidence.RequiresHumanReview() ? InvoiceStatus.PendingReview : InvoiceStatus.PendingReview;
        Raise(new InvoiceExtractedViaAiEvent(Id, confidence.Value, DateTime.UtcNow));
    }

    public void Approve()
    {
        if (Status == InvoiceStatus.Rejected) throw new DomainException("Rejected invoice cannot be approved.");
        Status = InvoiceStatus.Approved;
        Raise(new InvoiceApprovedDomainEvent(Id, DateTime.UtcNow));
    }

    public void Reject(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason)) throw new DomainException("Rejection reason is required.");
        if (Status == InvoiceStatus.Approved) throw new DomainException("Approved invoice cannot be rejected.");
        RejectionReason = reason;
        Status = InvoiceStatus.Rejected;
        Raise(new InvoiceRejectedDomainEvent(Id, reason, DateTime.UtcNow));
    }
}

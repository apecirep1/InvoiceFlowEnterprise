namespace InvoiceFlow.Domain.Invoices;

public enum InvoiceStatus
{
    PendingExtraction = 0,
    PendingReview = 1,
    Approved = 2,
    Rejected = 3,
    Failed = 4
}

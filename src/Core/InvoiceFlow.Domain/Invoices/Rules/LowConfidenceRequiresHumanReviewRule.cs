using InvoiceFlow.Domain.Invoices.ValueObjects;

namespace InvoiceFlow.Domain.Invoices.Rules;

public static class LowConfidenceRequiresHumanReviewRule
{
    public static bool IsSatisfied(ConfidenceScore score, decimal threshold = 0.85m)
        => score.RequiresHumanReview(threshold);
}

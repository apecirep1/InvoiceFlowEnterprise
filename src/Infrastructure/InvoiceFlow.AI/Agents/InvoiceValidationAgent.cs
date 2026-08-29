using InvoiceFlow.Domain.Invoices;

namespace InvoiceFlow.AI.Agents;

public sealed class InvoiceValidationAgent
{
    public IReadOnlyCollection<string> Validate(Invoice invoice)
    {
        var findings = new List<string>();
        if (invoice.Total.Amount <= 0) findings.Add("Invoice total must be positive.");
        if (invoice.ExtractionConfidence?.RequiresHumanReview() == true)
            findings.Add("Human review is required because extraction confidence is below threshold.");
        return findings;
    }
}

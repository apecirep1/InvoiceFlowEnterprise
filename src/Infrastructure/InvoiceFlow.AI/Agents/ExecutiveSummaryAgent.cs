namespace InvoiceFlow.AI.Agents;

public sealed class ExecutiveSummaryAgent
{
    public string Summarize(int pending, int approved, int rejected) =>
        $"InvoiceFlow executive summary: {pending} pending, {approved} approved and {rejected} rejected invoices.";
}

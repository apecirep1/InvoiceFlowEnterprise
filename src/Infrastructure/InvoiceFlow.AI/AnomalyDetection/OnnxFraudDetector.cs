using InvoiceFlow.Application.Abstractions.AI;
using InvoiceFlow.Domain.FraudAnalysis;
using InvoiceFlow.Domain.Invoices;

namespace InvoiceFlow.AI.AnomalyDetection;

public sealed class OnnxFraudDetector : IFraudDetectionModel
{
    public Task<RiskAssessment> AssessAsync(Invoice invoice, CancellationToken cancellationToken)
    {
        var flags = new List<FraudFlag>();
        decimal score = 0.10m;

        if (invoice.Total.Amount > 10000)
        {
            score += 0.35m;
            flags.Add(new FraudFlag("HIGH_AMOUNT", "Invoice amount exceeds the demo high-value threshold.", 0.35m));
        }
        if (invoice.ExtractionConfidence is { Value: < 0.80m })
        {
            score += 0.30m;
            flags.Add(new FraudFlag("LOW_AI_CONFIDENCE", "Extraction confidence is low.", 0.30m));
        }

        return Task.FromResult(RiskAssessment.FromScore(Math.Min(score, 1m), flags));
    }
}

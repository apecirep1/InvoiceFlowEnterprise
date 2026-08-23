namespace InvoiceFlow.Domain.FraudAnalysis;

public sealed record RiskAssessment(decimal Score, RiskLevel Level, IReadOnlyCollection<FraudFlag> Flags)
{
    public static RiskAssessment FromScore(decimal score, IEnumerable<FraudFlag>? flags = null)
    {
        var level = score switch
        {
            < 0.25m => RiskLevel.Low,
            < 0.50m => RiskLevel.Medium,
            < 0.75m => RiskLevel.High,
            _ => RiskLevel.Critical
        };
        return new(score, level, (flags ?? []).ToArray());
    }
}

namespace InvoiceFlow.Observability;

public static class LLMTracingExtensions
{
    public static decimal EstimateCost(int inputTokens, int outputTokens, decimal inputPerMillion = 0m, decimal outputPerMillion = 0m)
        => (inputTokens / 1_000_000m) * inputPerMillion + (outputTokens / 1_000_000m) * outputPerMillion;
}

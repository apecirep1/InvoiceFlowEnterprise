namespace InvoiceFlow.Domain.FraudAnalysis;
public sealed record FraudFlag(string Code, string Description, decimal Weight);

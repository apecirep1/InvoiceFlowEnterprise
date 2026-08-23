using InvoiceFlow.Domain.FraudAnalysis;
using InvoiceFlow.Domain.Invoices;

namespace InvoiceFlow.Application.Abstractions.AI;
public interface IFraudDetectionModel
{
    Task<RiskAssessment> AssessAsync(Invoice invoice, CancellationToken cancellationToken);
}

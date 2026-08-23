using InvoiceFlow.Domain.Common;
using InvoiceFlow.Domain.Exceptions;

namespace InvoiceFlow.Domain.Invoices.ValueObjects;

public sealed class ConfidenceScore : ValueObject
{
    public decimal Value { get; }

    private ConfidenceScore(decimal value)
    {
        if (value is < 0 or > 1) throw new DomainException("Confidence must be between 0 and 1.");
        Value = value;
    }

    public static ConfidenceScore From(decimal value) => new(value);

    public bool RequiresHumanReview(decimal threshold = 0.85m) => Value < threshold;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

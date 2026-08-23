using InvoiceFlow.Domain.Common;
using InvoiceFlow.Domain.Exceptions;

namespace InvoiceFlow.Domain.Invoices.ValueObjects;

public sealed class TaxNumber : ValueObject
{
    public string Value { get; }

    private TaxNumber(string value)
    {
        var normalized = value.Trim().Replace(" ", "");
        if (normalized.Length < 5) throw new DomainException("Tax number is invalid.");
        Value = normalized;
    }

    public static TaxNumber From(string value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}

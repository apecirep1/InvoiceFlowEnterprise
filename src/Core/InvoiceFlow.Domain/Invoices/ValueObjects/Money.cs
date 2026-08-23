using InvoiceFlow.Domain.Common;
using InvoiceFlow.Domain.Exceptions;

namespace InvoiceFlow.Domain.Invoices.ValueObjects;

public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        if (amount < 0) throw new DomainException("Amount cannot be negative.");
        Amount = decimal.Round(amount, 2);
        Currency = string.IsNullOrWhiteSpace(currency) ? "EUR" : currency.ToUpperInvariant();
    }

    public static Money Of(decimal amount, string currency = "EUR") => new(amount, currency);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:0.00} {Currency}";
}

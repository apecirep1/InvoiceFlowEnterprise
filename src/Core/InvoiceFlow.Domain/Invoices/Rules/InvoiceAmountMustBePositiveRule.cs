using InvoiceFlow.Domain.Invoices.ValueObjects;

namespace InvoiceFlow.Domain.Invoices.Rules;

public static class InvoiceAmountMustBePositiveRule
{
    public static bool IsSatisfied(Money total) => total.Amount > 0;
}

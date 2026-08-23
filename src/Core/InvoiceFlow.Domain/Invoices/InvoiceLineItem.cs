using InvoiceFlow.Domain.Common;
using InvoiceFlow.Domain.Invoices.ValueObjects;

namespace InvoiceFlow.Domain.Invoices;

public sealed class InvoiceLineItem : BaseEntity
{
    private InvoiceLineItem() { }

    public InvoiceLineItem(string description, decimal quantity, Money unitPrice)
    {
        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public string Description { get; private set; } = string.Empty;
    public decimal Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = Money.Of(0);
    public decimal LineTotal => Quantity * UnitPrice.Amount;
}

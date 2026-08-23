using InvoiceFlow.Domain.Common;
using InvoiceFlow.Domain.Invoices.ValueObjects;

namespace InvoiceFlow.Domain.Vendors;

public sealed class Vendor : BaseEntity
{
    private Vendor() { }
    public Vendor(string name, TaxNumber taxNumber, VendorAddress address)
    {
        Name = name;
        TaxNumber = taxNumber;
        Address = address;
    }
    public string Name { get; private set; } = string.Empty;
    public TaxNumber TaxNumber { get; private set; } = TaxNumber.From("UNKNOWN");
    public VendorAddress Address { get; private set; } = new("", "", "", "");
}

using FluentAssertions;
using InvoiceFlow.Domain.Invoices;
using InvoiceFlow.Domain.Invoices.ValueObjects;
using Xunit;

namespace InvoiceFlow.Domain.UnitTests.Invoices;

public sealed class InvoiceRuleTests
{
    [Fact]
    public void LowConfidence_Requires_HumanReview()
    {
        var invoice = new Invoice("INV-1", "Vendor", Money.Of(100), "invoice.pdf");
        invoice.ApplyExtraction(ConfidenceScore.From(0.50m));
        invoice.Status.Should().Be(InvoiceStatus.PendingReview);
    }

    [Fact]
    public void Invoice_Can_Be_Approved()
    {
        var invoice = new Invoice("INV-2", "Vendor", Money.Of(100), "invoice.pdf");
        invoice.ApplyExtraction(ConfidenceScore.From(0.95m));
        invoice.Approve();
        invoice.Status.Should().Be(InvoiceStatus.Approved);
    }
}

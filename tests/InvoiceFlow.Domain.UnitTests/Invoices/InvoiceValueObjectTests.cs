using FluentAssertions;
using InvoiceFlow.Domain.Invoices.ValueObjects;
using Xunit;

namespace InvoiceFlow.Domain.UnitTests.Invoices;

public sealed class InvoiceValueObjectTests
{
    [Fact]
    public void Money_Normalizes_Currency() =>
        Money.Of(10.123m, "eur").Currency.Should().Be("EUR");
}

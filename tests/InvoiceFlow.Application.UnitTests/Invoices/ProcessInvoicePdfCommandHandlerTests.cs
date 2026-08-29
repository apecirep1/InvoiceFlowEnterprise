using FluentAssertions;
using InvoiceFlow.Application.Invoices.Commands.ProcessInvoicePdf;
using Xunit;

namespace InvoiceFlow.Application.UnitTests.Invoices;
public sealed class ProcessInvoicePdfCommandHandlerTests
{
    [Fact]
    public void Validator_Requires_Pdf()
    {
        var validator = new ProcessInvoicePdfCommandValidator();
        var result = validator.Validate(new ProcessInvoicePdfCommand(new MemoryStream([1,2,3]), "invoice.txt"));
        result.IsValid.Should().BeFalse();
    }
}

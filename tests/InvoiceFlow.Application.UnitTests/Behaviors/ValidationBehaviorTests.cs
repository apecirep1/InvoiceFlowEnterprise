using FluentAssertions;
using InvoiceFlow.Application.Invoices.Commands.ApproveInvoice;
using Xunit;

namespace InvoiceFlow.Application.UnitTests.Behaviors;

public sealed class ValidationBehaviorTests
{
    [Fact]
    public void Validator_Rejects_Empty_Id()
    {
        var validator = new ApproveInvoiceCommandValidator();
        validator.Validate(new ApproveInvoiceCommand(Guid.Empty)).IsValid.Should().BeFalse();
    }
}

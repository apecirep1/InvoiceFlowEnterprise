using FluentValidation;
namespace InvoiceFlow.Application.Invoices.Commands.ApproveInvoice;
public sealed class ApproveInvoiceCommandValidator : AbstractValidator<ApproveInvoiceCommand>
{
    public ApproveInvoiceCommandValidator() => RuleFor(x => x.InvoiceId).NotEmpty();
}

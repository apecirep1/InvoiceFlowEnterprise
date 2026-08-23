using FluentValidation;
namespace InvoiceFlow.Application.Invoices.Commands.ProcessInvoicePdf;
public sealed class ProcessInvoicePdfCommandValidator : AbstractValidator<ProcessInvoicePdfCommand>
{
    public ProcessInvoicePdfCommandValidator()
    {
        RuleFor(x => x.Document).NotNull();
        RuleFor(x => x.FileName).NotEmpty().Must(x => x.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Only PDF invoices are accepted.");
    }
}

using InvoiceFlow.Application.Abstractions;
using InvoiceFlow.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.Application.Invoices.Commands.ApproveInvoice;

public sealed class ApproveInvoiceCommandHandler(IApplicationDbContext db)
    : IRequestHandler<ApproveInvoiceCommand>
{
    public async Task Handle(ApproveInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await db.Invoices.FirstOrDefaultAsync(x => x.Id == request.InvoiceId, cancellationToken)
                      ?? throw new InvoiceNotFoundException(request.InvoiceId);
        invoice.Approve();
        await db.SaveChangesAsync(cancellationToken);
    }
}

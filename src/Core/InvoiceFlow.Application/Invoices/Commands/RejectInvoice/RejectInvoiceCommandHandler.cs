using InvoiceFlow.Application.Abstractions;
using InvoiceFlow.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.Application.Invoices.Commands.RejectInvoice;

public sealed class RejectInvoiceCommandHandler(IApplicationDbContext db)
    : IRequestHandler<RejectInvoiceCommand>
{
    public async Task Handle(RejectInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await db.Invoices.FirstOrDefaultAsync(x => x.Id == request.InvoiceId, cancellationToken)
                      ?? throw new InvoiceNotFoundException(request.InvoiceId);
        invoice.Reject(request.Reason);
        await db.SaveChangesAsync(cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvoiceFlow.Infrastructure.Persistence.Outbox;

public sealed class OutboxProcessor(ApplicationDbContext db, ILogger<OutboxProcessor> logger)
{
    public async Task<int> ProcessAsync(CancellationToken cancellationToken)
    {
        var messages = await db.OutboxMessages
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(100)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            message.ProcessedOnUtc = DateTime.UtcNow;
            logger.LogInformation("Processed outbox message {Id} of type {Type}", message.Id, message.Type);
        }

        await db.SaveChangesAsync(cancellationToken);
        return messages.Count;
    }
}

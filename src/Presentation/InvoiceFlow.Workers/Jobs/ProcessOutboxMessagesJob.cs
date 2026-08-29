using InvoiceFlow.Infrastructure.Persistence.Outbox;
using Quartz;

namespace InvoiceFlow.Workers.Jobs;

public sealed class ProcessOutboxMessagesJob(OutboxProcessor processor) : IJob
{
    public async Task Execute(IJobExecutionContext context) =>
        _ = await processor.ProcessAsync(context.CancellationToken);
}

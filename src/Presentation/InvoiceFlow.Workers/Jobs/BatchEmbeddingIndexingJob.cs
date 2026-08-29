using Quartz;
namespace InvoiceFlow.Workers.Jobs;
public sealed class BatchEmbeddingIndexingJob : IJob
{
    public Task Execute(IJobExecutionContext context) => Task.CompletedTask;
}

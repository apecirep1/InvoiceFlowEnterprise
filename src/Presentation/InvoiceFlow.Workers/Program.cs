using InvoiceFlow.Infrastructure;
using InvoiceFlow.Workers.Jobs;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddQuartz(q =>
{
    var key = new JobKey("outbox");
    q.AddJob<ProcessOutboxMessagesJob>(o => o.WithIdentity(key));
    q.AddTrigger(t => t.ForJob(key).WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever()));
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

await builder.Build().RunAsync();

using InvoiceFlow.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace InvoiceFlow.Infrastructure.Services;
public sealed class SmtpEmailService(ILogger<SmtpEmailService> logger) : IEmailService
{
    public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken)
    {
        logger.LogInformation("Email (demo) to {To}: {Subject}", to, subject);
        return Task.CompletedTask;
    }
}

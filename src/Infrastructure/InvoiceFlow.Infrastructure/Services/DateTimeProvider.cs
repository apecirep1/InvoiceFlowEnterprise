using InvoiceFlow.Application.Abstractions;
namespace InvoiceFlow.Infrastructure.Services;
public sealed class DateTimeProvider : IDateTimeProvider { public DateTime UtcNow => DateTime.UtcNow; }

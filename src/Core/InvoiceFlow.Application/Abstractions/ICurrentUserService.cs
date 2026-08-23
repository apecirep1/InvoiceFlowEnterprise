namespace InvoiceFlow.Application.Abstractions;
public interface ICurrentUserService
{
    string UserId { get; }
    string TenantId { get; }
}

using InvoiceFlow.Application.Abstractions;

namespace InvoiceFlow.Infrastructure.Identity;

public sealed class CurrentUserService : ICurrentUserService
{
    public string UserId => "demo-user";
    public string TenantId => "demo-tenant";
}

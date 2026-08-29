namespace InvoiceFlow.WebApi.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/demo-login", () => Results.Ok(new
        {
            accessToken = "demo-local-token",
            expiresIn = 3600,
            note = "Development-only token. Replace with Keycloak/OIDC for production."
        })).WithTags("Auth");
        return app;
    }
}

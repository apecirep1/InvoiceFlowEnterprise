using InvoiceFlow.Application.Invoices.Queries.SemanticSearch;
using MediatR;

namespace InvoiceFlow.WebApi.Endpoints;

public static class AiAssistantEndpoints
{
    public static IEndpointRouteBuilder MapAiAssistantEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ai").WithTags("AI Assistant");

        group.MapGet("/search", async (string q, ISender sender, CancellationToken ct) =>
            Results.Ok(await sender.Send(new SemanticSearchQuery(q), ct)));

        group.MapGet("/explain", (string invoiceNumber) =>
            Results.Ok(new
            {
                invoiceNumber,
                explanation = "Demo AI explanation: risk combines invoice amount, extraction confidence, duplicate-like patterns and vendor history when those signals are available."
            }));

        return app;
    }
}

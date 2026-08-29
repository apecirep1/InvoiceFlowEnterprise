using InvoiceFlow.Application.Invoices.Commands.ApproveInvoice;
using InvoiceFlow.Application.Invoices.Commands.ProcessInvoicePdf;
using InvoiceFlow.Application.Invoices.Commands.RejectInvoice;
using InvoiceFlow.Application.Invoices.Queries.GetInvoiceById;
using InvoiceFlow.Application.Invoices.Queries.GetPendingInvoices;
using MediatR;

namespace InvoiceFlow.WebApi.Endpoints;

public static class InvoiceEndpoints
{
    public static IEndpointRouteBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/invoices").WithTags("Invoices");

        group.MapPost("/upload", async (IFormFile file, ISender sender, CancellationToken ct) =>
        {
            if (file.Length == 0) return Results.BadRequest(new { error = "Empty file." });
            await using var stream = file.OpenReadStream();
            var id = await sender.Send(new ProcessInvoicePdfCommand(stream, file.FileName), ct);
            return Results.Accepted($"/api/invoices/{id}", new { id });
        }).DisableAntiforgery();

        group.MapGet("/pending", async (ISender sender, CancellationToken ct) =>
            Results.Ok(await sender.Send(new GetPendingInvoicesQuery(), ct)));

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetInvoiceByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/{id:guid}/approve", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new ApproveInvoiceCommand(id), ct);
            return Results.NoContent();
        });

        group.MapPost("/{id:guid}/reject", async (Guid id, RejectRequest request, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new RejectInvoiceCommand(id, request.Reason), ct);
            return Results.NoContent();
        });

        return app;
    }

    public sealed record RejectRequest(string Reason);
}

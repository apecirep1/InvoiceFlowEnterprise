using InvoiceFlow.AI;
using InvoiceFlow.Application;
using InvoiceFlow.Infrastructure;
using InvoiceFlow.Infrastructure.Persistence;
using InvoiceFlow.Observability;
using InvoiceFlow.WebApi.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddInvoiceFlowLogging();
builder.Services.AddInvoiceFlowTelemetry();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddInvoiceFlowAi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "InvoiceFlow.WebApi" }));
app.MapInvoiceEndpoints();
app.MapAiAssistantEndpoints();
app.MapAuthEndpoints();

app.Run();

public partial class Program;

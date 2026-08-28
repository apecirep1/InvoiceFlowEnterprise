using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace InvoiceFlow.Infrastructure.Persistence.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS vector;");
    }
    protected override void Down(MigrationBuilder migrationBuilder) { }
}

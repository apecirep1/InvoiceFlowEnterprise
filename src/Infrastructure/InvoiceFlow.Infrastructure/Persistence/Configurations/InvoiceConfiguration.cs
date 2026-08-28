using InvoiceFlow.Domain.Invoices;
using InvoiceFlow.Domain.Invoices.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceFlow.Infrastructure.Persistence.Configurations;

public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("invoices");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.InvoiceNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.VendorName).HasMaxLength(200).IsRequired();
        builder.OwnsOne(x => x.Total, money =>
        {
            money.Property(x => x.Amount).HasColumnName("total_amount").HasPrecision(18,2);
            money.Property(x => x.Currency).HasColumnName("currency").HasMaxLength(3);
        });
        builder.OwnsOne(x => x.ExtractionConfidence);
        builder.Ignore(x => x.DomainEvents);
        builder.Ignore(x => x.LineItems);
    }
}

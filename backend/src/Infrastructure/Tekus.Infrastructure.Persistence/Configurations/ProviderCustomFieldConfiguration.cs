using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;

namespace Tekus.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for ProviderCustomField entity
/// </summary>
public class ProviderCustomFieldConfiguration : IEntityTypeConfiguration<ProviderCustomField>
{
    public void Configure(EntityTypeBuilder<ProviderCustomField> builder)
    {
        // Table name
        builder.ToTable("ProviderCustomFields");

        // Primary key
        builder.HasKey(cf => cf.Id);

        // Properties
        builder.Property(cf => cf.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(cf => cf.ProviderId)
            .IsRequired();

        builder.Property(cf => cf.FieldName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cf => cf.FieldValue)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(cf => cf.FieldType)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("string");

        builder.Property(cf => cf.CreatedAt)
            .IsRequired();

        builder.Property(cf => cf.UpdatedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(cf => new { cf.ProviderId, cf.FieldName })
            .IsUnique()
            .HasDatabaseName("IX_ProviderCustomFields_ProviderId_FieldName");

        // Ignore domain events
        builder.Ignore(cf => cf.DomainEvents);
    }
}

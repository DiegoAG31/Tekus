using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;
using Tekus.Infrastructure.Persistence.Converters;

namespace Tekus.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for Provider entity
/// </summary>
public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        // Table name
        builder.ToTable("Providers");

        // Primary key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.Id)
            .IsRequired()
            .ValueGeneratedNever(); // We generate GUIDs in domain

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        // Value Objects with Converters
        builder.Property(p => p.Nit)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(new NitConverter())
            .HasColumnName("Nit");

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasConversion(new EmailConverter())
            .HasColumnName("Email");

        // Indexes
        builder.HasIndex(p => p.Nit)
            .IsUnique()
            .HasDatabaseName("IX_Providers_Nit");

        builder.HasIndex(p => p.Email)
            .HasDatabaseName("IX_Providers_Email");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Providers_IsActive");

        // Relationships
        builder.HasMany(p => p.CustomFields)
            .WithOne(cf => cf.Provider)
            .HasForeignKey(cf => cf.ProviderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore domain events (not persisted)
        builder.Ignore(p => p.DomainEvents);
    }
}

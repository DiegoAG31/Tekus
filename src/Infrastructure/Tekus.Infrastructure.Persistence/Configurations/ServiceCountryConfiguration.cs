using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tekus.Core.Domain.Aggregates.ServiceAggregate;

namespace Tekus.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for ServiceCountry entity (Junction table)
/// </summary>
public class ServiceCountryConfiguration : IEntityTypeConfiguration<ServiceCountry>
{
    public void Configure(EntityTypeBuilder<ServiceCountry> builder)
    {
        // Table name
        builder.ToTable("ServiceCountries");

        // Primary key
        builder.HasKey(sc => sc.Id);

        // Properties
        builder.Property(sc => sc.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(sc => sc.ServiceId)
            .IsRequired();

        builder.Property(sc => sc.CountryCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(sc => sc.CreatedAt)
            .IsRequired();

        builder.Property(sc => sc.UpdatedAt)
            .IsRequired();

        // Composite unique index (Service + Country)
        builder.HasIndex(sc => new { sc.ServiceId, sc.CountryCode })
            .IsUnique()
            .HasDatabaseName("IX_ServiceCountries_ServiceId_CountryCode");

        // Index on CountryCode for reverse lookup
        builder.HasIndex(sc => sc.CountryCode)
            .HasDatabaseName("IX_ServiceCountries_CountryCode");

        // Relationships
        builder.HasOne<Service>() // Relaciona con la entidad Service
            .WithMany()
            .HasForeignKey(sc => sc.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ignore domain events
        builder.Ignore(sc => sc.DomainEvents);
    }
}

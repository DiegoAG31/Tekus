using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tekus.Core.Domain.Aggregates.ServiceAggregate;

namespace Tekus.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for Service entity
/// </summary>
public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        // Table name
        builder.ToTable("Services");

        // Primary key
        builder.HasKey(s => s.Id);

        // Properties
        builder.Property(s => s.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.ProviderId)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired();

        // Money Value Object as Owned Entity
        builder.OwnsOne(s => s.HourlyRate, money =>
        {
            money.Property(m => m.Amount)
                .IsRequired()
                .HasColumnName("HourlyRate_Amount")
                .HasColumnType("decimal(18,2)");

            money.Property(m => m.Currency)
                .IsRequired()
                .HasColumnName("HourlyRate_Currency")
                .HasMaxLength(3)
                .HasDefaultValue("USD");
        });

        // Indexes
        builder.HasIndex(s => s.ProviderId)
            .HasDatabaseName("IX_Services_ProviderId");

        builder.HasIndex(s => s.Name)
            .HasDatabaseName("IX_Services_Name");

        // Relationships
        builder.HasMany(s => s.ServiceCountries)
            .WithOne(sc => sc.Service)
            .HasForeignKey(sc => sc.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore domain events and private collections
        builder.Ignore(s => s.DomainEvents);
    }
}

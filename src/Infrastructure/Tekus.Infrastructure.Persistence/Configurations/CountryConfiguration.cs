using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tekus.Core.Domain.Entities;

namespace Tekus.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for Country entity
/// </summary>
public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        // Table name
        builder.ToTable("Countries");

        // Primary key (Code is the PK, not Id)
        builder.HasKey(c => c.Code);

        // Properties
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LastSync)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(c => c.Name)
            .HasDatabaseName("IX_Countries_Name");

        builder.HasIndex(c => c.LastSync)
            .HasDatabaseName("IX_Countries_LastSync");
    }
}

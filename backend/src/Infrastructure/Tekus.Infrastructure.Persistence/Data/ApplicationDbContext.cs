using Microsoft.EntityFrameworkCore;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;
using Tekus.Core.Domain.Aggregates.ServiceAggregate;
using Tekus.Core.Domain.Entities;

namespace Tekus.Infrastructure.Persistence.Data;

/// <summary>
/// Main DbContext for the application
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<ProviderCustomField> ProviderCustomFields => Set<ProviderCustomField>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ServiceCountry> ServiceCountries => Set<ServiceCountry>();
    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
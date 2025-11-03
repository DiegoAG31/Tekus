using Tekus.Core.Domain.Exceptions;

namespace Tekus.Core.Domain.Entities;

/// <summary>
/// Country Entity
/// Represents a country where services can be offered
/// Data is synced from external API
/// </summary>
public class Country
{
    public string Code { get; private set; } = string.Empty; // Primary Key (ISO 3166-1 alpha-3)
    public string Name { get; private set; } = string.Empty;
    public DateTime LastSync { get; private set; }

    private Country() { } // Para EF Core

    private Country(string code, string name)
    {
        Code = code.ToUpperInvariant();
        Name = name;
        LastSync = DateTime.UtcNow;
    }

    /// <summary>
    /// Factory method to create a new Country
    /// </summary>
    public static Country Create(string code, string name)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Country code cannot be empty");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Country name cannot be empty");

        if (code.Trim().Length != 3)
            throw new DomainException("Country code must be exactly 3 characters (ISO 3166-1 alpha-3)");

        return new Country(code, name);
    }

    /// <summary>
    /// Updates the country name (from API sync)
    /// </summary>
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Country name cannot be empty");

        Name = name;
        LastSync = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the last sync timestamp
    /// </summary>
    public void UpdateSync()
    {
        LastSync = DateTime.UtcNow;
    }
}
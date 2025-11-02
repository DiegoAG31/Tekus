using Tekus.Core.Domain.Entities;

namespace Tekus.Core.Domain.Repositories;

/// <summary>
/// Repository interface for Country entity
/// </summary>
public interface ICountryRepository
{
    /// <summary>
    /// Gets a country by its code
    /// </summary>
    Task<Country?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all countries
    /// </summary>
    Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds or updates a country (for sync from external API)
    /// </summary>
    Task UpsertAsync(Country country, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds or updates multiple countries (bulk sync)
    /// </summary>
    Task UpsertManyAsync(IEnumerable<Country> countries, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches countries by name
    /// </summary>
    Task<IEnumerable<Country>> SearchByNameAsync(
        string searchTerm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a country code exists
    /// </summary>
    Task<bool> ExistsAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets countries that need sync (older than specified hours)
    /// </summary>
    Task<IEnumerable<Country>> GetCountriesNeedingSyncAsync(
        int hoursThreshold = 24,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Unit of Work for transaction management
    /// </summary>
    IUnitOfWork UnitOfWork { get; }
}
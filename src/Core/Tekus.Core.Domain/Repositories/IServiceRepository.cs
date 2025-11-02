using Tekus.Core.Domain.Aggregates.ServiceAggregate;

namespace Tekus.Core.Domain.Repositories;

/// <summary>
/// Repository interface for Service aggregate
/// </summary>
public interface IServiceRepository : IGenericRepository<Service>
{
    /// <summary>
    /// Gets a service with all its country assignments
    /// </summary>
    Task<Service?> GetByIdWithCountriesAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all services for a specific provider
    /// </summary>
    Task<IEnumerable<Service>> GetByProviderIdAsync(
        Guid providerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets services offered in a specific country
    /// </summary>
    Task<IEnumerable<Service>> GetByCountryCodeAsync(
        string countryCode,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches services by name or description
    /// </summary>
    Task<IEnumerable<Service>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets active services with pagination
    /// </summary>
    Task<(IEnumerable<Service> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Guid? providerId = null,
        string? countryCode = null,
        string? searchTerm = null,
        string? sortBy = null,
        bool sortDescending = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets services count by country for analytics
    /// </summary>
    Task<Dictionary<string, int>> GetServiceCountByCountryAsync(
        CancellationToken cancellationToken = default);
}
using Tekus.Core.Domain.Aggregates.ProviderAggregate;

namespace Tekus.Core.Domain.Repositories;

/// <summary>
/// Repository interface for Provider aggregate
/// </summary>
public interface IProviderRepository : IGenericRepository<Provider>
{
    /// <summary>
    /// Gets a provider by its NIT
    /// </summary>
    Task<Provider?> GetByNitAsync(string nit, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a provider with all its custom fields
    /// </summary>
    Task<Provider?> GetByIdWithCustomFieldsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches providers by name or NIT
    /// </summary>
    Task<IEnumerable<Provider>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets active providers with pagination
    /// </summary>
    Task<(IEnumerable<Provider> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool sortDescending = false,
        CancellationToken cancellationToken = default);
}
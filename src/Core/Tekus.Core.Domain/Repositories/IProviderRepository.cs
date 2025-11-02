using Tekus.Core.Domain.Aggregates.ProviderAggregate;

namespace Tekus.Core.Domain.Repositories;

public interface IProviderRepository : IGenericRepository<Provider>
{
    Task<Provider?> GetByNitAsync(string nit, CancellationToken cancellationToken = default);
    Task<Provider?> GetByIdWithCustomFieldsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Provider>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Provider> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default);
}
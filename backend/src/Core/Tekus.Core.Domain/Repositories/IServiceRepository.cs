using Tekus.Core.Domain.Aggregates.ServiceAggregate;

namespace Tekus.Core.Domain.Repositories;

public interface IServiceRepository : IGenericRepository<Service>
{
    Task<Service?> GetByIdWithCountriesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Service>> GetByProviderIdAsync(Guid providerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Service>> GetByCountryCodeAsync(string countryCode, CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetServiceCountByCountryAsync(CancellationToken cancellationToken = default);
}
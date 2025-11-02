using Tekus.Core.Domain.Entities;

namespace Tekus.Core.Domain.Repositories;

public interface ICountryRepository
{
    Task<Country?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<Country, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Country>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Country>> GetCountriesNeedingSyncAsync(int hoursThreshold = 24, CancellationToken cancellationToken = default);
    Task UpsertAsync(Country country, CancellationToken cancellationToken = default);
    Task UpsertManyAsync(IEnumerable<Country> countries, CancellationToken cancellationToken = default);
    Task AddAsync(Country entity, CancellationToken cancellationToken = default);
}
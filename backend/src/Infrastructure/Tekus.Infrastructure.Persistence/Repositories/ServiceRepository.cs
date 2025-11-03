using Microsoft.EntityFrameworkCore;
using System.Linq;
using Tekus.Core.Domain.Aggregates.ServiceAggregate;
using Tekus.Core.Domain.Repositories;
using Tekus.Infrastructure.Persistence.Data;

namespace Tekus.Infrastructure.Persistence.Repositories;

public class ServiceRepository : GenericRepository<Service>, IServiceRepository
{
    public ServiceRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Service?> GetByIdWithCountriesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.ServiceCountries)
                .ThenInclude(sc => sc.CountryCode)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Service>> GetByProviderIdAsync(
        Guid providerId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.ServiceCountries)
                .ThenInclude(sc => sc.CountryCode)
            .Where(s => s.ProviderId == providerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Service>> GetByCountryCodeAsync(
        string countryCode,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.ServiceCountries)
                .ThenInclude(sc => sc.CountryCode)
            .Where(s => s.ServiceCountries.Any(sc => sc.CountryCode == countryCode))
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<string, int>> GetServiceCountByCountryAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.ServiceCountries
            .GroupBy(sc => sc.CountryCode)
            .Select(g => new { CountryCode = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CountryCode, x => x.Count, cancellationToken);
    }
}
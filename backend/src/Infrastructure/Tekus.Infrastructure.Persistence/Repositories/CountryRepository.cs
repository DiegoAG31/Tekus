using Microsoft.EntityFrameworkCore;
using Tekus.Core.Domain.Entities;
using Tekus.Core.Domain.Repositories;
using Tekus.Infrastructure.Persistence.Data;

namespace Tekus.Infrastructure.Persistence.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Country> _dbSet;

    public CountryRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<Country>();
    }

    public async Task<Country?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { code }, cancellationToken);
    }

    public async Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.OrderBy(c => c.Name).ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(
        System.Linq.Expressions.Expression<Func<Country, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<Country>> SearchByNameAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.Name.Contains(searchTerm))
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(c => c.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<Country>> GetCountriesNeedingSyncAsync(
        int hoursThreshold = 24,
        CancellationToken cancellationToken = default)
    {
        var cutoffDate = DateTime.UtcNow.AddHours(-hoursThreshold);

        return await _dbSet
            .Where(c => c.LastSync == default(DateTime) || c.LastSync < cutoffDate)
            .ToListAsync(cancellationToken);
    }

    public async Task UpsertAsync(Country country, CancellationToken cancellationToken = default)
    {
        var existing = await GetByCodeAsync(country.Code, cancellationToken);

        if (existing == null)
        {
            await _dbSet.AddAsync(country, cancellationToken);
        }
        else
        {
            existing.UpdateName(country.Name);
            existing.UpdateSync();
            _dbSet.Update(existing);
        }
    }

    public async Task UpsertManyAsync(
        IEnumerable<Country> countries,
        CancellationToken cancellationToken = default)
    {
        foreach (var country in countries)
        {
            await UpsertAsync(country, cancellationToken);
        }
    }

    public async Task AddAsync(Country entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }
}
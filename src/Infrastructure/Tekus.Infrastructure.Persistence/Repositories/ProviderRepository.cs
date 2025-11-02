using Microsoft.EntityFrameworkCore;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;
using Tekus.Core.Domain.Repositories;
using Tekus.Core.Domain.ValueObjects;
using Tekus.Infrastructure.Persistence.Data;

namespace Tekus.Infrastructure.Persistence.Repositories;

public class ProviderRepository : GenericRepository<Provider>, IProviderRepository
{
    public ProviderRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Provider?> GetByNitAsync(string nit, CancellationToken cancellationToken = default)
    {
        var nitValue = Nit.Create(nit);
        return await _dbSet
            .Include(p => p.CustomFields)
            .FirstOrDefaultAsync(p => p.Nit == nitValue, cancellationToken);
    }

    public async Task<Provider?> GetByIdWithCustomFieldsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.CustomFields)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Provider>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.CustomFields)
            .Where(p => p.Name.Contains(searchTerm))
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Provider> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Include(p => p.CustomFields).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm));
        }

        if (isActive.HasValue)
        {
            query = query.Where(p => p.IsActive == isActive.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(p => p.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
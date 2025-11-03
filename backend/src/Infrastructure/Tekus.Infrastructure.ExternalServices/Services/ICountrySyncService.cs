namespace Tekus.Infrastructure.ExternalServices.Services;

/// <summary>
/// Service for synchronizing countries from external API to database
/// </summary>
public interface ICountrySyncService
{
    /// <summary>
    /// Synchronizes all countries from external API to local database
    /// </summary>
    Task<int> SyncAllCountriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Synchronizes countries that haven't been synced recently
    /// </summary>
    Task<int> SyncOutdatedCountriesAsync(int daysOld = 30, CancellationToken cancellationToken = default);
}

using Microsoft.Extensions.Logging;
using Tekus.Core.Domain.Entities;
using Tekus.Core.Domain.Repositories;
using Tekus.Infrastructure.ExternalServices.ApiClients;

namespace Tekus.Infrastructure.ExternalServices.Services;

/// <summary>
/// Service for synchronizing countries from external API to database
/// </summary>
public class CountrySyncService : ICountrySyncService
{
    private readonly ICountryApiClient _countryApiClient;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CountrySyncService> _logger;

    public CountrySyncService(
        ICountryApiClient countryApiClient,
        ICountryRepository countryRepository,
        IUnitOfWork unitOfWork,
        ILogger<CountrySyncService> logger)
    {
        _countryApiClient = countryApiClient;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<int> SyncAllCountriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting full country synchronization");

            var apiCountries = await _countryApiClient.GetAllCountriesAsync(cancellationToken);
            var syncedCount = 0;

            foreach (var apiCountry in apiCountries)
            {
                if (string.IsNullOrEmpty(apiCountry.Cca3) || apiCountry.Name?.Common == null)
                    continue;

                var country = Country.Create(apiCountry.Cca3, apiCountry.Name.Common);
                await _countryRepository.UpsertAsync(country, cancellationToken);
                syncedCount++;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully synchronized {Count} countries", syncedCount);
            return syncedCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during country synchronization");
            throw;
        }
    }

    public async Task<int> SyncOutdatedCountriesAsync(int daysOld = 30, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting outdated countries synchronization (older than {Days} days)", daysOld);

            // Cambiar cutoffDate a hoursThreshold (int) para coincidir con la firma del repositorio
            int hoursThreshold = daysOld * 24;
            var outdatedCountries = await _countryRepository.GetCountriesNeedingSyncAsync(hoursThreshold, cancellationToken);

            if (!outdatedCountries.Any())
            {
                _logger.LogInformation("No outdated countries found");
                return 0;
            }

            var syncedCount = 0;

            foreach (var outdatedCountry in outdatedCountries)
            {
                var apiCountry = await _countryApiClient.GetCountryByCodeAsync(outdatedCountry.Code, cancellationToken);

                if (apiCountry != null && apiCountry.Name?.Common != null)
                {
                    outdatedCountry.UpdateName(apiCountry.Name.Common);
                    outdatedCountry.UpdateSync();
                    syncedCount++;
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully synchronized {Count} outdated countries", syncedCount);
            return syncedCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during outdated countries synchronization");
            throw;
        }
    }
}

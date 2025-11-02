using Tekus.Infrastructure.ExternalServices.ApiClients.Models;

namespace Tekus.Infrastructure.ExternalServices.ApiClients;

/// <summary>
/// Client for consuming Countries REST API
/// </summary>
public interface ICountryApiClient
{
    /// <summary>
    /// Fetches all countries from the external API
    /// </summary>
    Task<IEnumerable<CountryApiResponse>> GetAllCountriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Fetches a country by its ISO 3166-1 alpha-3 code
    /// </summary>
    Task<CountryApiResponse?> GetCountryByCodeAsync(string code, CancellationToken cancellationToken = default);
}

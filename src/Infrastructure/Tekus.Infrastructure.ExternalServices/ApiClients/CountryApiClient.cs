using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using Tekus.Infrastructure.ExternalServices.ApiClients.Models;

namespace Tekus.Infrastructure.ExternalServices.ApiClients;

/// <summary>
/// Country API Client implementation using REST Countries API
/// https://restcountries.com
/// </summary>
public class CountryApiClient : ICountryApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CountryApiClient> _logger;
    private const string BaseUrl = "https://restcountries.com/v3.1";

    public CountryApiClient(HttpClient httpClient, ILogger<CountryApiClient> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
        _logger = logger;
    }

    public async Task<IEnumerable<CountryApiResponse>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching all countries from REST Countries API");

            var response = await _httpClient.GetAsync("/all?fields=name,cca2,cca3", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to fetch countries. Status: {StatusCode}", response.StatusCode);
                return Enumerable.Empty<CountryApiResponse>();
            }

            var countries = await response.Content.ReadFromJsonAsync<List<CountryApiResponse>>(cancellationToken);

            _logger.LogInformation("Successfully fetched {Count} countries", countries?.Count ?? 0);

            return countries ?? Enumerable.Empty<CountryApiResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching countries from external API");
            throw;
        }
    }

    public async Task<CountryApiResponse?> GetCountryByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching country with code: {Code}", code);

            var response = await _httpClient.GetAsync($"/alpha/{code}?fields=name,cca2,cca3", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Country with code {Code} not found. Status: {StatusCode}", code, response.StatusCode);
                return null;
            }

            var countries = await response.Content.ReadFromJsonAsync<List<CountryApiResponse>>(cancellationToken);
            var country = countries?.FirstOrDefault();

            if (country != null)
            {
                _logger.LogInformation("Successfully fetched country: {Name}", country.Name?.Common);
            }

            return country;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching country {Code} from external API", code);
            throw;
        }
    }
}

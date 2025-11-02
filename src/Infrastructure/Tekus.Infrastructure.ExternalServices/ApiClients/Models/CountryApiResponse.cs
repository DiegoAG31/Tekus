namespace Tekus.Infrastructure.ExternalServices.ApiClients.Models;

/// <summary>
/// Response model from REST Countries API
/// https://restcountries.com/v3.1/all
/// </summary>
public class CountryApiResponse
{
    public CountryName? Name { get; set; }
    public string? Cca3 { get; set; } // ISO 3166-1 alpha-3 code
    public string? Cca2 { get; set; } // ISO 3166-1 alpha-2 code
}

public class CountryName
{
    public string? Common { get; set; }
    public string? Official { get; set; }
}

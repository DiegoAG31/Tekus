using Microsoft.Extensions.DependencyInjection;
using Tekus.Infrastructure.ExternalServices.ApiClients;
using Tekus.Infrastructure.ExternalServices.Services;

namespace Tekus.Infrastructure.ExternalServices;

public static class DependencyInjection
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        // Register HttpClient for Country API
        services.AddHttpClient<ICountryApiClient, CountryApiClient>();

        // Register Country Sync Service
        services.AddScoped<ICountrySyncService, CountrySyncService>();

        return services;
    }
}

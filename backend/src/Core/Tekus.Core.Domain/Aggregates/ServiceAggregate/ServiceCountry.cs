using Tekus.Core.Domain.Common;

namespace Tekus.Core.Domain.Aggregates.ServiceAggregate;

/// <summary>
/// Represents the relationship between a Service and a Country
/// Part of the Service aggregate
/// A service can be offered in multiple countries
/// </summary>
public class ServiceCountry : BaseEntity
{
    public Guid ServiceId { get; private set; }
    public string CountryCode { get; private set; } = string.Empty;

    // Navigation properties
    public Service Service { get; private set; } = null!;

    private ServiceCountry() { } // Para EF Core

    private ServiceCountry(Guid serviceId, string countryCode)
    {
        Id = Guid.NewGuid();
        ServiceId = serviceId;
        CountryCode = countryCode;
        CreatedAt = DateTime.UtcNow;
    }

    internal static ServiceCountry Create(Guid serviceId, string countryCode)
    {
        return new ServiceCountry(serviceId, countryCode);
    }
}
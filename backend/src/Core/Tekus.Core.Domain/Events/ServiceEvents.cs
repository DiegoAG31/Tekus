using Tekus.Core.Domain.Common;

namespace Tekus.Core.Domain.Events;

/// <summary>
/// Event raised when a service is created
/// </summary>
public class ServiceCreatedEvent : IDomainEvent
{
    public Guid ServiceId { get; }
    public Guid ProviderId { get; }
    public string ServiceName { get; }
    public DateTime OccurredOn { get; }

    public ServiceCreatedEvent(Guid serviceId, Guid providerId, string serviceName)
    {
        ServiceId = serviceId;
        ProviderId = providerId;
        ServiceName = serviceName;
        OccurredOn = DateTime.UtcNow;
    }
}

/// <summary>
/// Event raised when a country is assigned to a service
/// </summary>
public class ServiceCountryAssignedEvent : IDomainEvent
{
    public Guid ServiceId { get; }
    public string CountryCode { get; }
    public DateTime OccurredOn { get; }

    public ServiceCountryAssignedEvent(Guid serviceId, string countryCode)
    {
        ServiceId = serviceId;
        CountryCode = countryCode;
        OccurredOn = DateTime.UtcNow;
    }
}

/// <summary>
/// Event raised when a service changes provider
/// </summary>
public class ServiceProviderChangedEvent : IDomainEvent
{
    public Guid ServiceId { get; }
    public Guid OldProviderId { get; }
    public Guid NewProviderId { get; }
    public DateTime OccurredOn { get; }

    public ServiceProviderChangedEvent(Guid serviceId, Guid oldProviderId, Guid newProviderId)
    {
        ServiceId = serviceId;
        OldProviderId = oldProviderId;
        NewProviderId = newProviderId;
        OccurredOn = DateTime.UtcNow;
    }
}
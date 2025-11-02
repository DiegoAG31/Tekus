using Tekus.Core.Domain.Common;

namespace Tekus.Core.Domain.Events;

/// <summary>
/// Event raised when a provider is created
/// </summary>
public class ProviderCreatedEvent : IDomainEvent
{
    public Guid ProviderId { get; }
    public string ProviderName { get; }
    public string Nit { get; }
    public DateTime OccurredOn { get; }

    public ProviderCreatedEvent(Guid providerId, string providerName, string nit)
    {
        ProviderId = providerId;
        ProviderName = providerName;
        Nit = nit;
        OccurredOn = DateTime.UtcNow;
    }
}

/// <summary>
/// Event raised when a provider is deactivated
/// </summary>
public class ProviderDeactivatedEvent : IDomainEvent
{
    public Guid ProviderId { get; }
    public DateTime OccurredOn { get; }

    public ProviderDeactivatedEvent(Guid providerId)
    {
        ProviderId = providerId;
        OccurredOn = DateTime.UtcNow;
    }
}

/// <summary>
/// Event raised when a custom field is added to a provider
/// </summary>
public class ProviderCustomFieldAddedEvent : IDomainEvent
{
    public Guid ProviderId { get; }
    public string FieldName { get; }
    public DateTime OccurredOn { get; }

    public ProviderCustomFieldAddedEvent(Guid providerId, string fieldName)
    {
        ProviderId = providerId;
        FieldName = fieldName;
        OccurredOn = DateTime.UtcNow;
    }
}
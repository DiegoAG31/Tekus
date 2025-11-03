namespace Tekus.Core.Domain.Common;

/// <summary>
/// Marker interface to identify Aggregate Roots in DDD
/// Aggregate Roots can raise domain events
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// Collection of domain events raised by this aggregate
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Adds a domain event to the aggregate
    /// </summary>
    void AddDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Removes a domain event from the aggregate
    /// </summary>
    void RemoveDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Clears all domain events from the aggregate
    /// </summary>
    void ClearDomainEvents();
}
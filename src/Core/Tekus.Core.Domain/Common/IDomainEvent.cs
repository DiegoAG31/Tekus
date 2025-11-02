namespace Tekus.Core.Domain.Common;

/// <summary>
/// Base interface for all domain events
/// Domain events represent something that happened in the domain
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// When the event occurred
    /// </summary>
    DateTime OccurredOn { get; }
}
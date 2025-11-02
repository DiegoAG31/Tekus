using FluentAssertions;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;
using Tekus.Core.Domain.Common;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.UnitTests.Domain.Common;

/// <summary>
/// Tests for Domain Events functionality
/// </summary>
public class DomainEventsTests
{
    [Fact]
    public void NewAggregate_ShouldHaveNoDomainEvents()
    {
        // Arrange & Act
        var domainEvent = new TestDomainEvent();
        var provider = CreateValidProvider();

        // Assert
        provider.DomainEvents.Should().NotContain(domainEvent);

    }

    [Fact]
    public void AddDomainEvent_ShouldAddEventToCollection()
    {
        // Arrange
        var provider = CreateValidProvider();
        var domainEvent = new TestDomainEvent();

        // Act
        provider.AddDomainEvent(domainEvent);

        // Assert
        provider.DomainEvents.Should().HaveCount(2);
        provider.DomainEvents.Should().Contain(domainEvent);
    }

    [Fact]
    public void AddMultipleDomainEvents_ShouldAddAllEvents()
    {
        // Arrange
        var provider = CreateValidProvider();
        var event1 = new TestDomainEvent();
        var event2 = new TestDomainEvent();

        // Act
        provider.AddDomainEvent(event1);
        provider.AddDomainEvent(event2);

        // Assert
        provider.DomainEvents.Should().HaveCount(3);
    }

    [Fact]
    public void RemoveDomainEvent_ShouldRemoveEventFromCollection()
    {
        // Arrange
        var provider = CreateValidProvider();
        var domainEvent = new TestDomainEvent();
        provider.AddDomainEvent(domainEvent);

        // Act
        provider.RemoveDomainEvent(domainEvent);

        // Assert
        provider.DomainEvents.Should().HaveCount(1);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        // Arrange
        var provider = CreateValidProvider();
        provider.AddDomainEvent(new TestDomainEvent());
        provider.AddDomainEvent(new TestDomainEvent());

        // Act
        provider.ClearDomainEvents();

        // Assert
        provider.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void DomainEvents_ShouldBeReadOnly()
    {
        // Arrange
        var provider = CreateValidProvider();

        // Assert
        provider.DomainEvents.Should().BeAssignableTo<IReadOnlyCollection<IDomainEvent>>();
    }

    // Helper classes
    private class TestDomainEvent : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }

    private Provider CreateValidProvider()
    {
        var nit = Nit.Create("900123456-7");
        var name = "Test Provider";
        var email = Email.Create("test@tekus.com");
        return Provider.Create(nit, name, email);
    }
}
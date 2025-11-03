using FluentAssertions;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;
using Tekus.Core.Domain.Specifications;
using Tekus.Core.Domain.ValueObjects;
using Xunit;

namespace Tekus.UnitTests.Domain.Specifications;

/// <summary>
/// Tests for Provider Specifications following TDD approach
/// Specifications encapsulate query logic that can be reused
/// </summary>
public class ProviderSpecificationsTests
{
    [Fact]
    public void ActiveProvidersSpec_ShouldReturnOnlyActiveProviders()
    {
        // Arrange
        var activeProvider = CreateProvider("Active Provider", true);
        var inactiveProvider = CreateProvider("Inactive Provider", false);
        var providers = new[] { activeProvider, inactiveProvider };

        var spec = ProviderSpecifications.ActiveProviders();

        // Act
        var result = providers.Where(spec.ToExpression().Compile());

        // Assert
        result.Should().HaveCount(1);
        result.First().Should().Be(activeProvider);
    }

    [Fact]
    public void ByNitSpec_ShouldReturnProviderWithMatchingNit()
    {
        // Arrange
        var nit1 = "900123456-7";
        var nit2 = "800987654-3";
        var provider1 = CreateProvider("Provider 1", nit: nit1);
        var provider2 = CreateProvider("Provider 2", nit: nit2);
        var providers = new[] { provider1, provider2 };

        var spec = ProviderSpecifications.ByNit(nit1);

        // Act
        var result = providers.Where(spec.ToExpression().Compile());

        // Assert
        result.Should().HaveCount(1);
        result.First().Nit.Value.Should().Be(nit1);
    }

    [Fact]
    public void ByEmailSpec_ShouldReturnProviderWithMatchingEmail()
    {
        // Arrange
        var email1 = "test1@tekus.com";
        var email2 = "test2@tekus.com";
        var provider1 = CreateProvider("Provider 1", email: email1);
        var provider2 = CreateProvider("Provider 2", email: email2);
        var providers = new[] { provider1, provider2 };

        var spec = ProviderSpecifications.ByEmail(email1);

        // Act
        var result = providers.Where(spec.ToExpression().Compile());

        // Assert
        result.Should().HaveCount(1);
        result.First().Email.Value.Should().Be(email1);
    }

    [Fact]
    public void SearchByNameSpec_ShouldReturnProvidersContainingSearchTerm()
    {
        // Arrange
        var provider1 = CreateProvider("Importaciones Tekus");
        var provider2 = CreateProvider("Tekus Services");
        var provider3 = CreateProvider("Other Company");
        var providers = new[] { provider1, provider2, provider3 };

        var spec = ProviderSpecifications.SearchByName("Tekus");

        // Act
        var result = providers.Where(spec.ToExpression().Compile());

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(provider1);
        result.Should().Contain(provider2);
    }

    [Fact]
    public void SearchByNameSpec_ShouldBeCaseInsensitive()
    {
        // Arrange
        var provider = CreateProvider("Importaciones TEKUS");
        var providers = new[] { provider };

        var spec = ProviderSpecifications.SearchByName("tekus");

        // Act
        var result = providers.Where(spec.ToExpression().Compile());

        // Assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public void AndSpec_ShouldCombineTwoSpecifications()
    {
        // Arrange
        var activeProvider = CreateProvider("Active Tekus", true);
        var inactiveProvider = CreateProvider("Inactive Tekus", false);
        var activeOther = CreateProvider("Active Other", true);
        var providers = new[] { activeProvider, inactiveProvider, activeOther };

        var activeSpec = ProviderSpecifications.ActiveProviders();
        var nameSpec = ProviderSpecifications.SearchByName("Tekus");
        var combinedSpec = activeSpec.And(nameSpec);

        // Act
        var result = providers.Where(combinedSpec.ToExpression().Compile());

        // Assert
        result.Should().HaveCount(1);
        result.First().Should().Be(activeProvider);
    }

    [Fact]
    public void OrSpec_ShouldCombineTwoSpecificationsWithOr()
    {
        // Arrange
        var provider1 = CreateProvider("Tekus Company", false);
        var provider2 = CreateProvider("Other Company", true);
        var provider3 = CreateProvider("Another Company", false);
        var providers = new[] { provider1, provider2, provider3 };

        var activeSpec = ProviderSpecifications.ActiveProviders();
        var nameSpec = ProviderSpecifications.SearchByName("Tekus");
        var combinedSpec = activeSpec.Or(nameSpec);

        // Act
        var result = providers.Where(combinedSpec.ToExpression().Compile());

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(provider1); // Matches name
        result.Should().Contain(provider2); // Is active
    }

    [Fact]
    public void NotSpec_ShouldInvertSpecification()
    {
        // Arrange
        var activeProvider = CreateProvider("Active Provider", true);
        var inactiveProvider = CreateProvider("Inactive Provider", false);
        var providers = new[] { activeProvider, inactiveProvider };

        var activeSpec = ProviderSpecifications.ActiveProviders();
        var notActiveSpec = activeSpec.Not();

        // Act
        var result = providers.Where(notActiveSpec.ToExpression().Compile());

        // Assert
        result.Should().HaveCount(1);
        result.First().Should().Be(inactiveProvider);
    }

    // Helper methods
    private Provider CreateProvider(
        string name,
        bool isActive = true,
        string? nit = null,
        string? email = null)
    {
        var nitValue = nit ?? $"900{new Random().Next(100000, 999999)}-7";
        var emailValue = email ?? $"test{Guid.NewGuid().ToString().Substring(0, 8)}@tekus.com";

        var provider = Provider.Create(
            Nit.Create(nitValue),
            name,
            Email.Create(emailValue)
        );

        if (!isActive)
            provider.Deactivate();

        return provider;
    }
}
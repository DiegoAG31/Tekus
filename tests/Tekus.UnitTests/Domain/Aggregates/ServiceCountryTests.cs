using FluentAssertions;
using Tekus.Core.Domain.Aggregates.ServiceAggregate;
using Tekus.Core.Domain.Exceptions;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.UnitTests.Domain.Aggregates;

/// <summary>
/// Tests for Service Country assignments
/// Service can be offered in multiple countries (N:M relationship)
/// </summary>
public class ServiceCountryTests
{
    [Fact]
    public void AssignCountry_WithValidCountryCode_ShouldSucceed()
    {
        // Arrange
        var service = CreateValidService();
        var countryCode = "COL";

        // Act
        service.AssignCountry(countryCode);

        // Assert
        service.ServiceCountries.Should().HaveCount(1);
        var serviceCountry = service.ServiceCountries.First();
        serviceCountry.CountryCode.Should().Be(countryCode);
        serviceCountry.ServiceId.Should().Be(service.Id);
        service.UpdatedAt.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AssignCountry_WithNullOrEmptyCountryCode_ShouldThrowDomainException(string? invalidCode)
    {
        // Arrange
        var service = CreateValidService();

        // Act
        Action act = () => service.AssignCountry(invalidCode!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Country code cannot be empty*");
    }

    [Fact]
    public void AssignCountry_WithDuplicateCountryCode_ShouldThrowDomainException()
    {
        // Arrange
        var service = CreateValidService();
        var countryCode = "COL";
        service.AssignCountry(countryCode);

        // Act
        Action act = () => service.AssignCountry(countryCode);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage($"*{countryCode}*already assigned*");
    }

    [Fact]
    public void AssignMultipleCountries_ShouldSucceed()
    {
        // Arrange
        var service = CreateValidService();

        // Act
        service.AssignCountry("COL");
        service.AssignCountry("PER");
        service.AssignCountry("MEX");

        // Assert
        service.ServiceCountries.Should().HaveCount(3);
        service.ServiceCountries.Select(sc => sc.CountryCode)
            .Should().BeEquivalentTo(new[] { "COL", "PER", "MEX" });
    }

    [Fact]
    public void RemoveCountry_WithValidCountryCode_ShouldSucceed()
    {
        // Arrange
        var service = CreateValidService();
        var countryCode = "COL";
        service.AssignCountry(countryCode);

        // Act
        service.RemoveCountry(countryCode);

        // Assert
        service.ServiceCountries.Should().BeEmpty();
        service.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void RemoveCountry_WithNonExistentCountryCode_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var service = CreateValidService();
        var countryCode = "COL";

        // Act
        Action act = () => service.RemoveCountry(countryCode);

        // Assert
        act.Should().Throw<EntityNotFoundException>()
            .WithMessage($"*{countryCode}*not found*");
    }

    [Fact]
    public void GetCountryCodes_ShouldReturnAllAssignedCountries()
    {
        // Arrange
        var service = CreateValidService();
        service.AssignCountry("COL");
        service.AssignCountry("PER");
        service.AssignCountry("MEX");

        // Act
        var countryCodes = service.GetCountryCodes();

        // Assert
        countryCodes.Should().BeEquivalentTo(new[] { "COL", "PER", "MEX" });
    }

    [Fact]
    public void IsOfferedInCountry_WithAssignedCountry_ShouldReturnTrue()
    {
        // Arrange
        var service = CreateValidService();
        var countryCode = "COL";
        service.AssignCountry(countryCode);

        // Act
        var isOffered = service.IsOfferedInCountry(countryCode);

        // Assert
        isOffered.Should().BeTrue();
    }

    [Fact]
    public void IsOfferedInCountry_WithNonAssignedCountry_ShouldReturnFalse()
    {
        // Arrange
        var service = CreateValidService();
        service.AssignCountry("COL");

        // Act
        var isOffered = service.IsOfferedInCountry("PER");

        // Assert
        isOffered.Should().BeFalse();
    }

    [Fact]
    public void ServiceCountries_ShouldBeReadOnly()
    {
        // Arrange
        var service = CreateValidService();

        // Assert
        service.ServiceCountries.Should().BeAssignableTo<IReadOnlyCollection<ServiceCountry>>();
    }

    // Helper method
    private Service CreateValidService()
    {
        var providerId = Guid.NewGuid();
        var name = "Test Service";
        var hourlyRate = Money.Create(150m);
        return Service.Create(providerId, name, hourlyRate);
    }
}
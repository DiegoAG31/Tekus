using FluentAssertions;
using Tekus.Core.Domain.Aggregates.ServiceAggregate;
using Tekus.Core.Domain.Exceptions;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.UnitTests.Domain.Aggregates;

/// <summary>
/// Tests for Service Aggregate Root following TDD approach
/// Service belongs to one Provider (N:1 relationship)
/// Service can be offered in multiple countries (N:M with Country)
/// </summary>
public class ServiceTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        // Arrange
        var providerId = Guid.NewGuid();
        var name = "Descarga espacial de contenidos";
        var hourlyRate = Money.Create(150.50m);
        var description = "Servicio de descarga espacial";

        // Act
        var service = Service.Create(providerId, name, hourlyRate, description);

        // Assert
        service.Should().NotBeNull();
        service.Id.Should().NotBe(Guid.Empty);
        service.ProviderId.Should().Be(providerId);
        service.Name.Should().Be(name);
        service.HourlyRate.Should().Be(hourlyRate);
        service.Description.Should().Be(description);
        service.IsActive.Should().BeTrue();
        service.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        service.UpdatedAt.Should().BeNull();
        service.ServiceCountries.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithoutDescription_ShouldSucceed()
    {
        // Arrange
        var providerId = Guid.NewGuid();
        var name = "Test Service";
        var hourlyRate = Money.Create(100m);

        // Act
        var service = Service.Create(providerId, name, hourlyRate);

        // Assert
        service.Should().NotBeNull();
        service.Description.Should().BeNull();
    }

    [Fact]
    public void Create_WithEmptyProviderId_ShouldThrowDomainException()
    {
        // Arrange
        var providerId = Guid.Empty;
        var name = "Test Service";
        var hourlyRate = Money.Create(100m);

        // Act
        Action act = () => Service.Create(providerId, name, hourlyRate);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*ProviderId cannot be empty*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrEmptyName_ShouldThrowDomainException(string? invalidName)
    {
        // Arrange
        var providerId = Guid.NewGuid();
        var hourlyRate = Money.Create(100m);

        // Act
        Action act = () => Service.Create(providerId, invalidName!, hourlyRate);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Service name cannot be empty*");
    }

    [Fact]
    public void Create_WithNullHourlyRate_ShouldThrowArgumentNullException()
    {
        // Arrange
        var providerId = Guid.NewGuid();
        var name = "Test Service";

        // Act
        Action act = () => Service.Create(providerId, name, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("hourlyRate");
    }

    [Fact]
    public void Update_WithValidData_ShouldSucceed()
    {
        // Arrange
        var service = CreateValidService();
        var newName = "Updated Service Name";
        var newHourlyRate = Money.Create(200m);
        var newDescription = "Updated description";

        // Act
        service.Update(newName, newHourlyRate, newDescription);

        // Assert
        service.Name.Should().Be(newName);
        service.HourlyRate.Should().Be(newHourlyRate);
        service.Description.Should().Be(newDescription);
        service.UpdatedAt.Should().NotBeNull();
        service.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Update_WithEmptyName_ShouldThrowDomainException()
    {
        // Arrange
        var service = CreateValidService();
        var hourlyRate = Money.Create(100m);

        // Act
        Action act = () => service.Update("  ", hourlyRate, null);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Service name cannot be empty*");
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        // Arrange
        var service = CreateValidService();

        // Act
        service.Deactivate();

        // Assert
        service.IsActive.Should().BeFalse();
        service.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Activate_ShouldSetIsActiveTrue()
    {
        // Arrange
        var service = CreateValidService();
        service.Deactivate();

        // Act
        service.Activate();

        // Assert
        service.IsActive.Should().BeTrue();
        service.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void ChangeProvider_WithValidProviderId_ShouldSucceed()
    {
        // Arrange
        var service = CreateValidService();
        var newProviderId = Guid.NewGuid();

        // Act
        service.ChangeProvider(newProviderId);

        // Assert
        service.ProviderId.Should().Be(newProviderId);
        service.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void ChangeProvider_WithEmptyProviderId_ShouldThrowDomainException()
    {
        // Arrange
        var service = CreateValidService();

        // Act
        Action act = () => service.ChangeProvider(Guid.Empty);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*ProviderId cannot be empty*");
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
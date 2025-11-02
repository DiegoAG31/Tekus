using FluentAssertions;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;
using Tekus.Core.Domain.Exceptions;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.UnitTests.Domain.Aggregates;

/// <summary>
/// Tests for Provider Aggregate Root following TDD approach
/// Provider is the aggregate root that can have many Services (1:N relationship)
/// </summary>
public class ProviderTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        // Arrange
        var nit = Nit.Create("900123456-7");
        var name = "Importaciones Tekus S.A.S.";
        var email = Email.Create("contacto@tekus.com");

        // Act
        var provider = Provider.Create(nit, name, email);

        // Assert
        provider.Should().NotBeNull();
        provider.Id.Should().NotBe(Guid.Empty);
        provider.Nit.Should().Be(nit);
        provider.Name.Should().Be(name);
        provider.Email.Should().Be(email);
        provider.IsActive.Should().BeTrue();
        provider.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        provider.UpdatedAt.Should().BeNull();
        provider.CustomFields.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithNullNit_ShouldThrowArgumentNullException()
    {
        // Arrange
        var name = "Test Provider";
        var email = Email.Create("test@tekus.com");

        // Act
        Action act = () => Provider.Create(null!, name, email);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("nit");
    }

    [Fact]
    public void Create_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var nit = Nit.Create("900123456-7");
        var email = Email.Create("test@tekus.com");

        // Act
        Action act = () => Provider.Create(nit, null!, email);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("name");
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowDomainException()
    {
        // Arrange
        var nit = Nit.Create("900123456-7");
        var email = Email.Create("test@tekus.com");

        // Act
        Action act = () => Provider.Create(nit, "  ", email);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Provider name cannot be empty*");
    }

    [Fact]
    public void Create_WithNullEmail_ShouldThrowArgumentNullException()
    {
        // Arrange
        var nit = Nit.Create("900123456-7");
        var name = "Test Provider";

        // Act
        Action act = () => Provider.Create(nit, name, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("email");
    }

    [Fact]
    public void Update_WithValidData_ShouldSucceed()
    {
        // Arrange
        var provider = CreateValidProvider();
        var newName = "Updated Provider Name";
        var newEmail = Email.Create("updated@tekus.com");

        // Act
        provider.Update(newName, newEmail);

        // Assert
        provider.Name.Should().Be(newName);
        provider.Email.Should().Be(newEmail);
        provider.UpdatedAt.Should().NotBeNull();
        provider.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Update_WithEmptyName_ShouldThrowDomainException()
    {
        // Arrange
        var provider = CreateValidProvider();
        var email = Email.Create("test@tekus.com");

        // Act
        Action act = () => provider.Update("  ", email);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Provider name cannot be empty*");
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        // Arrange
        var provider = CreateValidProvider();

        // Act
        provider.Deactivate();

        // Assert
        provider.IsActive.Should().BeFalse();
        provider.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Activate_ShouldSetIsActiveTrue()
    {
        // Arrange
        var provider = CreateValidProvider();
        provider.Deactivate();

        // Act
        provider.Activate();

        // Assert
        provider.IsActive.Should().BeTrue();
        provider.UpdatedAt.Should().NotBeNull();
    }

    // Helper method
    private Provider CreateValidProvider()
    {
        var nit = Nit.Create("900123456-7");
        var name = "Test Provider";
        var email = Email.Create("test@tekus.com");
        return Provider.Create(nit, name, email);
    }
}
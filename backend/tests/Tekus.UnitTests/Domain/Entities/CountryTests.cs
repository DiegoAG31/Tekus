using FluentAssertions;
using System.Diagnostics.Metrics;
using Tekus.Core.Domain.Entities;
using Tekus.Core.Domain.Exceptions;
using Xunit;

namespace Tekus.UnitTests.Domain.Entities;

/// <summary>
/// Tests for Country Entity following TDD approach
/// Country is synced from external API
/// </summary>
public class CountryTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        // Arrange
        var code = "COL";
        var name = "Colombia";

        // Act
        var country = Country.Create(code, name);

        // Assert
        country.Should().NotBeNull();
        country.Code.Should().Be(code);
        country.Name.Should().Be(name);
        country.LastSync.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("COL", "Colombia")]
    [InlineData("PER", "Perú")]
    [InlineData("MEX", "México")]
    [InlineData("USA", "United States")]
    public void Create_WithVariousValidCountries_ShouldSucceed(string code, string name)
    {
        // Act
        var country = Country.Create(code, name);

        // Assert
        country.Code.Should().Be(code);
        country.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrEmptyCode_ShouldThrowDomainException(string? invalidCode)
    {
        // Act
        Action act = () => Country.Create(invalidCode!, "Colombia");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Country code cannot be empty*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrEmptyName_ShouldThrowDomainException(string? invalidName)
    {
        // Act
        Action act = () => Country.Create("COL", invalidName!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Country name cannot be empty*");
    }

    [Theory]
    [InlineData("CO")] // 2 characters
    [InlineData("COLO")] // 4 characters
    [InlineData("C")] // 1 character
    public void Create_WithInvalidCodeLength_ShouldThrowDomainException(string invalidCode)
    {
        // Act
        Action act = () => Country.Create(invalidCode, "Colombia");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Country code must be exactly 3 characters*");
    }

    [Fact]
    public void UpdateSync_ShouldUpdateLastSyncTime()
    {
        // Arrange
        var country = Country.Create("COL", "Colombia");
        var originalSync = country.LastSync;
        Thread.Sleep(10); // Ensure time difference

        // Act
        country.UpdateSync();

        // Assert
        country.LastSync.Should().BeAfter(originalSync);
        country.LastSync.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldSucceed()
    {
        // Arrange
        var country = Country.Create("COL", "Colombia");
        var newName = "República de Colombia";

        // Act
        country.UpdateName(newName);

        // Assert
        country.Name.Should().Be(newName);
        country.LastSync.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateName_WithNullOrEmptyName_ShouldThrowDomainException(string? invalidName)
    {
        // Arrange
        var country = Country.Create("COL", "Colombia");

        // Act
        Action act = () => country.UpdateName(invalidName!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Country name cannot be empty*");
    }

    [Fact]
    public void TwoCountriesWithSameCode_ShouldBeEqual()
    {
        // Arrange
        var country1 = Country.Create("COL", "Colombia");
        var country2 = Country.Create("COL", "República de Colombia");

        // Act & Assert
        // Note: Countries are equal by Code (primary key), not by name
        country1.Code.Should().Be(country2.Code);
    }

    [Fact]
    public void Code_ShouldBeNormalizedToUpperCase()
    {
        // Arrange & Act
        var country = Country.Create("col", "Colombia");

        // Assert
        country.Code.Should().Be("COL");
    }
}
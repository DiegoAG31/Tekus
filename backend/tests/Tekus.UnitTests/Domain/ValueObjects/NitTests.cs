using FluentAssertions;
using Moq;
using Tekus.Core.Domain.Exceptions;
using Tekus.Core.Domain.ValueObjects;
using Xunit;


namespace Tekus.UnitTests.Domain.ValueObjects;

/// <summary>
/// Tests for Nit Value Object following TDD approach
/// </summary>
public class NitTests
{
    [Fact]
    public void Create_WithValidNit_ShouldSucceed()
    {
        // Arrange
        var validNit = "900123456-7";

        // Act
        var nit = Nit.Create(validNit);

        // Assert
        nit.Should().NotBeNull();
        nit.Value.Should().Be("900123456-7");
    }

    [Theory]
    [InlineData("123456789")]
    [InlineData("900.123.456-7")]
    [InlineData("860123456-1")]
    [InlineData("NIT900123456")]
    public void Create_WithVariousValidNits_ShouldSucceed(string validNit)
    {
        // Act
        var nit = Nit.Create(validNit);

        // Assert
        nit.Should().NotBeNull();
        nit.Value.Should().Be(validNit);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrEmptyNit_ShouldThrowDomainException(string? invalidNit)
    {
        // Act
        Action act = () => Nit.Create(invalidNit!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*NIT cannot be empty*");
    }

    [Fact]
    public void Create_WithNitTooLong_ShouldThrowDomainException()
    {
        // Arrange
        var longNit = new string('1', 21); // 21 characters

        // Act
        Action act = () => Nit.Create(longNit);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*NIT cannot exceed 20 characters*");
    }

    [Fact]
    public void TwoNitsWithSameValue_ShouldBeEqual()
    {
        // Arrange
        var nit1 = Nit.Create("900123456-7");
        var nit2 = Nit.Create("900123456-7");

        // Act & Assert
        nit1.Should().Be(nit2);
        (nit1 == nit2).Should().BeTrue();
    }

    [Fact]
    public void TwoNitsWithDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var nit1 = Nit.Create("900123456-7");
        var nit2 = Nit.Create("800123456-7");

        // Act & Assert
        nit1.Should().NotBe(nit2);
        (nit1 != nit2).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnNitValue()
    {
        // Arrange
        var nit = Nit.Create("900123456-7");

        // Act
        var result = nit.ToString();

        // Assert
        result.Should().Be("900123456-7");
    }

    [Fact]
    public void ImplicitConversionToString_ShouldWork()
    {
        // Arrange
        var nit = Nit.Create("900123456-7");

        // Act
        string nitString = nit;

        // Assert
        nitString.Should().Be("900123456-7");
    }

    [Fact]
    public void GetHashCode_ForSameNits_ShouldBeEqual()
    {
        // Arrange
        var nit1 = Nit.Create("900123456-7");
        var nit2 = Nit.Create("900123456-7");

        // Act & Assert
        nit1.GetHashCode().Should().Be(nit2.GetHashCode());
    }
}
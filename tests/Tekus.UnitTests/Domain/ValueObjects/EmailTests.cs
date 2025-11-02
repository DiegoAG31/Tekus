using FluentAssertions;
using Tekus.Core.Domain.Exceptions;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.UnitTests.Domain.ValueObjects;

/// <summary>
/// Tests for Email Value Object following TDD approach
/// </summary>
public class EmailTests
{
    [Fact]
    public void Create_WithValidEmail_ShouldSucceed()
    {
        // Arrange
        var validEmail = "test@tekus.com";

        // Act
        var email = Email.Create(validEmail);

        // Assert
        email.Should().NotBeNull();
        email.Value.Should().Be("test@tekus.com");
    }

    [Theory]
    [InlineData("user@example.com")]
    [InlineData("test.user@domain.co")]
    [InlineData("admin@tekus.com.co")]
    [InlineData("contact+tag@email.org")]
    public void Create_WithVariousValidEmails_ShouldSucceed(string validEmail)
    {
        // Act
        var email = Email.Create(validEmail);

        // Assert
        email.Should().NotBeNull();
        email.Value.Should().Be(validEmail.ToLowerInvariant());
    }

    [Fact]
    public void Create_WithUpperCaseEmail_ShouldConvertToLowerCase()
    {
        // Arrange
        var upperCaseEmail = "TEST@TEKUS.COM";

        // Act
        var email = Email.Create(upperCaseEmail);

        // Assert
        email.Value.Should().Be("test@tekus.com");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrEmptyEmail_ShouldThrowDomainException(string? invalidEmail)
    {
        // Act
        Action act = () => Email.Create(invalidEmail!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Email cannot be empty*");
    }

    [Theory]
    [InlineData("notanemail")]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    [InlineData("user domain@email.com")]
    [InlineData("user@@domain.com")]
    public void Create_WithInvalidEmailFormat_ShouldThrowDomainException(string invalidEmail)
    {
        // Act
        Action act = () => Email.Create(invalidEmail);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage($"*'{invalidEmail}'*not valid*");
    }

    [Fact]
    public void TwoEmailsWithSameValue_ShouldBeEqual()
    {
        // Arrange
        var email1 = Email.Create("test@tekus.com");
        var email2 = Email.Create("test@tekus.com");

        // Act & Assert
        email1.Should().Be(email2);
        (email1 == email2).Should().BeTrue();
    }

    [Fact]
    public void TwoEmailsWithDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var email1 = Email.Create("test1@tekus.com");
        var email2 = Email.Create("test2@tekus.com");

        // Act & Assert
        email1.Should().NotBe(email2);
        (email1 != email2).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnEmailValue()
    {
        // Arrange
        var email = Email.Create("test@tekus.com");

        // Act
        var result = email.ToString();

        // Assert
        result.Should().Be("test@tekus.com");
    }

    [Fact]
    public void ImplicitConversionToString_ShouldWork()
    {
        // Arrange
        var email = Email.Create("test@tekus.com");

        // Act
        string emailString = email;

        // Assert
        emailString.Should().Be("test@tekus.com");
    }

    [Fact]
    public void GetHashCode_ForSameEmails_ShouldBeEqual()
    {
        // Arrange
        var email1 = Email.Create("test@tekus.com");
        var email2 = Email.Create("test@tekus.com");

        // Act & Assert
        email1.GetHashCode().Should().Be(email2.GetHashCode());
    }
}
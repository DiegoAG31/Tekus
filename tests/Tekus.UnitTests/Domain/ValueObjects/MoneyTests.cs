using FluentAssertions;
using Tekus.Core.Domain.Exceptions;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.UnitTests.Domain.ValueObjects;

/// <summary>
/// Tests for Money Value Object following TDD approach
/// </summary>
public class MoneyTests
{
    [Fact]
    public void Create_WithValidAmount_ShouldSucceed()
    {
        // Arrange
        var amount = 150.50m;

        // Act
        var money = Money.Create(amount);

        // Assert
        money.Should().NotBeNull();
        money.Amount.Should().Be(150.50m);
        money.Currency.Should().Be("USD");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.01)]
    [InlineData(100)]
    [InlineData(999999.99)]
    public void Create_WithVariousValidAmounts_ShouldSucceed(decimal amount)
    {
        // Act
        var money = Money.Create(amount);

        // Assert
        money.Should().NotBeNull();
        money.Amount.Should().Be(amount);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-100)]
    [InlineData(-999999.99)]
    public void Create_WithNegativeAmount_ShouldThrowDomainException(decimal negativeAmount)
    {
        // Act
        Action act = () => Money.Create(negativeAmount);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Amount cannot be negative*");
    }

    [Fact]
    public void Create_WithCustomCurrency_ShouldSucceed()
    {
        // Arrange
        var amount = 100m;
        var currency = "COP";

        // Act
        var money = Money.Create(amount, currency);

        // Assert
        money.Amount.Should().Be(100m);
        money.Currency.Should().Be("COP");
    }

    [Fact]
    public void TwoMoneyWithSameValues_ShouldBeEqual()
    {
        // Arrange
        var money1 = Money.Create(100m);
        var money2 = Money.Create(100m);

        // Act & Assert
        money1.Should().Be(money2);
        (money1 == money2).Should().BeTrue();
    }

    [Fact]
    public void TwoMoneyWithDifferentAmounts_ShouldNotBeEqual()
    {
        // Arrange
        var money1 = Money.Create(100m);
        var money2 = Money.Create(200m);

        // Act & Assert
        money1.Should().NotBe(money2);
        (money1 != money2).Should().BeTrue();
    }

    [Fact]
    public void TwoMoneyWithDifferentCurrencies_ShouldNotBeEqual()
    {
        // Arrange
        var money1 = Money.Create(100m, "USD");
        var money2 = Money.Create(100m, "COP");

        // Act & Assert
        money1.Should().NotBe(money2);
        (money1 != money2).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnFormattedMoney()
    {
        // Arrange
        var money = Money.Create(150.50m);

        // Act
        var result = money.ToString();

        // Assert
        result.Should().Be("$150,50 USD");
    }

    [Fact]
    public void Add_TwoMoneyWithSameCurrency_ShouldReturnSum()
    {
        // Arrange
        var money1 = Money.Create(100m);
        var money2 = Money.Create(50m);

        // Act
        var result = money1.Add(money2);

        // Assert
        result.Amount.Should().Be(150m);
    }

    [Fact]
    public void Add_TwoMoneyWithDifferentCurrencies_ShouldThrowDomainException()
    {
        // Arrange
        var money1 = Money.Create(100m, "USD");
        var money2 = Money.Create(50m, "COP");

        // Act
        Action act = () => money1.Add(money2);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Cannot add money with different currencies*");
    }

    [Fact]
    public void Subtract_TwoMoneyWithSameCurrency_ShouldReturnDifference()
    {
        // Arrange
        var money1 = Money.Create(100m);
        var money2 = Money.Create(30m);

        // Act
        var result = money1.Subtract(money2);

        // Assert
        result.Amount.Should().Be(70m);
    }

    [Fact]
    public void Multiply_ByFactor_ShouldReturnProduct()
    {
        // Arrange
        var money = Money.Create(50m);

        // Act
        var result = money.Multiply(3);

        // Assert
        result.Amount.Should().Be(150m);
    }

    [Fact]
    public void GetHashCode_ForSameMoney_ShouldBeEqual()
    {
        // Arrange
        var money1 = Money.Create(100m);
        var money2 = Money.Create(100m);

        // Act & Assert
        money1.GetHashCode().Should().Be(money2.GetHashCode());
    }
}
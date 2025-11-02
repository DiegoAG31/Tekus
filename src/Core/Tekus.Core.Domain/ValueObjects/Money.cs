using Tekus.Core.Domain.Common;
using Tekus.Core.Domain.Exceptions;

namespace Tekus.Core.Domain.ValueObjects;

/// <summary>
/// Money Value Object
/// Represents a monetary amount with currency
/// </summary>
public class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private Money()
    {
        Currency = "USD";
    }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Creates a new Money instance
    /// </summary>
    /// <param name="amount">Monetary amount</param>
    /// <param name="currency">Currency code (default: USD)</param>
    /// <returns>Valid Money instance</returns>
    /// <exception cref="DomainException">Thrown when amount is negative</exception>
    public static Money Create(decimal amount, string currency = "USD")
    {
        if (amount < 0)
        {
            throw new DomainException("Amount cannot be negative");
        }

        return new Money(amount, currency);
    }

    /// <summary>
    /// Adds two money amounts (must have same currency)
    /// </summary>
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
        {
            throw new DomainException("Cannot add money with different currencies");
        }

        return new Money(Amount + other.Amount, Currency);
    }

    /// <summary>
    /// Subtracts two money amounts (must have same currency)
    /// </summary>
    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
        {
            throw new DomainException("Cannot subtract money with different currencies");
        }

        return Create(Amount - other.Amount, Currency);
    }

    /// <summary>
    /// Multiplies money by a factor
    /// </summary>
    public Money Multiply(decimal factor)
    {
        return Create(Amount * factor, Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"${Amount:0.00} {Currency}";
}
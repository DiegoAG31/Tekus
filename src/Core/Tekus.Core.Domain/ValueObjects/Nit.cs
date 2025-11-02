using Tekus.Core.Domain.Common;
using Tekus.Core.Domain.Exceptions;

namespace Tekus.Core.Domain.ValueObjects;

/// <summary>
/// NIT (Número de Identificación Tributaria) Value Object
/// Represents a validated Colombian tax identification number
/// </summary>
public class Nit : ValueObject
{
    private const int MaxLength = 20;

    public string Value { get; private set; }

    private Nit()
    {
        Value = string.Empty;
    }

    private Nit(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new Nit instance
    /// </summary>
    /// <param name="nit">NIT to validate</param>
    /// <returns>Valid Nit instance</returns>
    /// <exception cref="DomainException">Thrown when NIT is invalid</exception>
    public static Nit Create(string nit)
    {
        if (string.IsNullOrWhiteSpace(nit))
        {
            throw new DomainException("NIT cannot be empty");
        }

        if (nit.Length > MaxLength)
        {
            throw new DomainException($"NIT cannot exceed {MaxLength} characters");
        }

        return new Nit(nit);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(Nit nit) => nit.Value;
}
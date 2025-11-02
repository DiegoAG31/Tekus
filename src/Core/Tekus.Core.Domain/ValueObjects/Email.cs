using System.Net.Mail;
using Tekus.Core.Domain.Common;
using Tekus.Core.Domain.Exceptions;

namespace Tekus.Core.Domain.ValueObjects;

/// <summary>
/// Email Value Object
/// Represents a validated email address
/// </summary>
public class Email : ValueObject
{
    public string Value { get; private set; }

    private Email()
    {
        Value = string.Empty;
    }

    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new Email instance
    /// </summary>
    /// <param name="email">Email address to validate</param>
    /// <returns>Valid Email instance</returns>
    /// <exception cref="DomainException">Thrown when email is invalid</exception>
    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email cannot be empty");
        }

        if (!IsValidEmail(email))
        {
            throw new DomainException($"Email '{email}' is not valid");
        }

        return new Email(email.ToLowerInvariant());
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}
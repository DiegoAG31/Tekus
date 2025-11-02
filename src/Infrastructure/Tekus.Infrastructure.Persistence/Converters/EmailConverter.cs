using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.Infrastructure.Persistence.Converters;

/// <summary>
/// EF Core Value Converter for Email Value Object
/// </summary>
public class EmailConverter : ValueConverter<Email, string>
{
    public EmailConverter()
        : base(
            email => email.Value,
            value => Email.Create(value))
    {
    }
}

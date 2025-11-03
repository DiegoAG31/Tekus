using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.Infrastructure.Persistence.Converters;

/// <summary>
/// EF Core Value Converter for Nit Value Object
/// </summary>
public class NitConverter : ValueConverter<Nit, string>
{
    public NitConverter()
        : base(
            nit => nit.Value,
            value => Nit.Create(value))
    {
    }
}

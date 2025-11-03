using Tekus.Core.Domain.Common;

namespace Tekus.Core.Domain.Aggregates.ProviderAggregate;

/// <summary>
/// Represents a custom field for a Provider
/// Part of the Provider aggregate
/// </summary>
public class ProviderCustomField : BaseEntity
{
    public Guid ProviderId { get; private set; }
    public string FieldName { get; private set; } = string.Empty;
    public string FieldValue { get; private set; } = string.Empty;
    public string FieldType { get; private set; } = string.Empty;

    // Navigation property
    public Provider Provider { get; private set; } = null!;

    private ProviderCustomField() { }

    private ProviderCustomField(Guid providerId, string fieldName, string fieldValue, string fieldType)
    {
        Id = Guid.NewGuid();
        ProviderId = providerId;
        FieldName = fieldName;
        FieldValue = fieldValue;
        FieldType = fieldType;
        CreatedAt = DateTime.UtcNow;
    }

    internal static ProviderCustomField Create(Guid providerId, string fieldName, string fieldValue, string fieldType)
    {
        return new ProviderCustomField(providerId, fieldName, fieldValue, fieldType);
    }

    internal void Update(string fieldValue, string fieldType)
    {
        FieldValue = fieldValue;
        FieldType = fieldType;
        UpdatedAt = DateTime.UtcNow;
    }
}
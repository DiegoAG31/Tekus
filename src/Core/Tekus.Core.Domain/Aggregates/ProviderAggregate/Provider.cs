using Tekus.Core.Domain.Common;
using Tekus.Core.Domain.Events;
using Tekus.Core.Domain.Exceptions;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.Core.Domain.Aggregates.ProviderAggregate;

/// <summary>
/// Provider Aggregate Root
/// A provider can have many services (1:N relationship)
/// A provider can have custom fields
/// </summary>
public class Provider : BaseEntity, IAggregateRoot
{
    private readonly List<ProviderCustomField> _customFields;

    public Nit Nit { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public bool IsActive { get; private set; }

    // Navigation properties
    public IReadOnlyCollection<ProviderCustomField> CustomFields => _customFields.AsReadOnly();

    private Provider() // Para EF Core
    {
        _customFields = new List<ProviderCustomField>();
    }

    private Provider(Nit nit, string name, Email email)
    {
        Id = Guid.NewGuid();
        Nit = nit;
        Name = name;
        Email = email;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        _customFields = new List<ProviderCustomField>();
    }

    /// <summary>
    /// Factory method to create a new Provider
    /// </summary>
    public static Provider Create(Nit nit, string name, Email email)
    {
        if (nit == null)
            throw new ArgumentNullException(nameof(nit));

        if (name == null)
            throw new ArgumentNullException(nameof(name));

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Provider name cannot be empty");

        if (email == null)
            throw new ArgumentNullException(nameof(email));

        var provider = new Provider(nit, name, email);

        // Raise domain event
        provider.AddDomainEvent(new ProviderCreatedEvent(
            provider.Id,
            provider.Name,
            provider.Nit.Value));

        return provider;
    }

    /// <summary>
    /// Updates provider information
    /// </summary>
    public void Update(string name, Email email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Provider name cannot be empty");

        Name = name;
        Email = email ?? throw new ArgumentNullException(nameof(email));
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds or updates a custom field
    /// </summary>
    public void AddCustomField(string fieldName, string fieldValue, string fieldType)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            throw new DomainException("Field name cannot be empty");

        var existingField = _customFields.FirstOrDefault(f => f.FieldName == fieldName);
        if (existingField != null)
        {
            existingField.Update(fieldValue, fieldType);
        }
        else
        {
            var customField = ProviderCustomField.Create(this.Id, fieldName, fieldValue, fieldType);
            _customFields.Add(customField);

            // Raise domain event
            AddDomainEvent(new ProviderCustomFieldAddedEvent(this.Id, fieldName));
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a custom field
    /// </summary>
    public void RemoveCustomField(Guid customFieldId)
    {
        var field = _customFields.FirstOrDefault(f => f.Id == customFieldId);
        if (field == null)
            throw new EntityNotFoundException($"Custom field with ID {customFieldId} not found");

        _customFields.Remove(field);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the provider
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        AddDomainEvent(new ProviderDeactivatedEvent(this.Id));
    }

    /// <summary>
    /// Activates the provider
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
using Tekus.Core.Domain.Common;
using Tekus.Core.Domain.Events;
using Tekus.Core.Domain.Exceptions;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.Core.Domain.Aggregates.ServiceAggregate;

/// <summary>
/// Service Aggregate Root
/// A service belongs to one Provider (N:1 relationship)
/// A service can be offered in multiple countries (N:M relationship)
/// </summary>
public class Service : BaseEntity, IAggregateRoot
{
    private readonly List<ServiceCountry> _serviceCountries;

    public Guid ProviderId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Money HourlyRate { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public IReadOnlyCollection<ServiceCountry> ServiceCountries => _serviceCountries.AsReadOnly();

    private Service() // Para EF Core
    {
        _serviceCountries = new List<ServiceCountry>();
    }

    private Service(Guid providerId, string name, Money hourlyRate, string? description = null)
    {
        Id = Guid.NewGuid();
        ProviderId = providerId;
        Name = name;
        HourlyRate = hourlyRate;
        Description = description;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        _serviceCountries = new List<ServiceCountry>();
    }

    /// <summary>
    /// Factory method to create a new Service
    /// </summary>
    public static Service Create(Guid providerId, string name, Money hourlyRate, string? description = null)
    {
        if (providerId == Guid.Empty)
            throw new DomainException("ProviderId cannot be empty");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Service name cannot be empty");

        if (hourlyRate == null)
            throw new ArgumentNullException(nameof(hourlyRate));

        var service = new Service(providerId, name, hourlyRate, description);

        // Raise domain event
        service.AddDomainEvent(new ServiceCreatedEvent(
            service.Id,
            service.ProviderId,
            service.Name));

        return service;
    }

    /// <summary>
    /// Updates service information
    /// </summary>
    public void Update(string name, Money hourlyRate, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Service name cannot be empty");

        if (hourlyRate == null)
            throw new ArgumentNullException(nameof(hourlyRate));

        Name = name;
        HourlyRate = hourlyRate;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Changes the provider that offers this service
    /// </summary>
    public void ChangeProvider(Guid newProviderId)
    {
        if (newProviderId == Guid.Empty)
            throw new DomainException("ProviderId cannot be empty");

        var oldProviderId = ProviderId;
        ProviderId = newProviderId;
        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        AddDomainEvent(new ServiceProviderChangedEvent(this.Id, oldProviderId, newProviderId));
    }

    /// <summary>
    /// Assigns a country where this service is offered
    /// </summary>
    public void AssignCountry(string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
            throw new DomainException("Country code cannot be empty");

        if (_serviceCountries.Any(sc => sc.CountryCode == countryCode))
            throw new DomainException($"Country {countryCode} is already assigned to this service");

        var serviceCountry = ServiceCountry.Create(this.Id, countryCode);
        _serviceCountries.Add(serviceCountry);
        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        AddDomainEvent(new ServiceCountryAssignedEvent(this.Id, countryCode));
    }

    /// <summary>
    /// Removes a country assignment
    /// </summary>
    public void RemoveCountry(string countryCode)
    {
        var serviceCountry = _serviceCountries.FirstOrDefault(sc => sc.CountryCode == countryCode);
        if (serviceCountry == null)
            throw new EntityNotFoundException($"Country {countryCode} not found for this service");

        _serviceCountries.Remove(serviceCountry);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets all country codes where this service is offered
    /// </summary>
    public IEnumerable<string> GetCountryCodes()
    {
        return _serviceCountries.Select(sc => sc.CountryCode);
    }

    /// <summary>
    /// Checks if the service is offered in a specific country
    /// </summary>
    public bool IsOfferedInCountry(string countryCode)
    {
        return _serviceCountries.Any(sc => sc.CountryCode == countryCode);
    }

    /// <summary>
    /// Deactivates the service
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activates the service
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
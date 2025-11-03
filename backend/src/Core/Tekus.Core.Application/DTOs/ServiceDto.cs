namespace Tekus.Core.Application.DTOs;

/// <summary>
/// DTO for Service entity
/// </summary>
public class ServiceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public string Currency { get; set; } = "USD";
    public Guid ProviderId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public List<string> CountryCodes { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
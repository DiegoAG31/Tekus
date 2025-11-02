namespace Tekus.Core.Application.DTOs;

/// <summary>
/// DTO for Country entity
/// </summary>
public class CountryDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime? LastSync { get; set; }
}
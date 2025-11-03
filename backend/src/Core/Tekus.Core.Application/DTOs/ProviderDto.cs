namespace Tekus.Core.Application.DTOs;

/// <summary>
/// DTO for Provider entity
/// </summary>
public class ProviderDto
{
    public Guid Id { get; set; }
    public string Nit { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<CustomFieldDto> CustomFields { get; set; } = new();
}

/// <summary>
/// DTO for Custom Field
/// </summary>
public class CustomFieldDto
{
    // ✅ Agregar 'required' o '= string.Empty'
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
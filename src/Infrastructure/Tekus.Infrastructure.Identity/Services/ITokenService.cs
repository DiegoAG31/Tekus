namespace Tekus.Infrastructure.Identity.Services;

/// <summary>
/// Service for generating JWT tokens
/// </summary>
public interface ITokenService
{
    string GenerateToken(string userId, string email, IEnumerable<string> roles);
    bool ValidateToken(string token);
}

namespace Tekus.Infrastructure.ExternalServices.Email;

/// <summary>
/// Service for sending emails
/// </summary>
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    Task SendEmailAsync(IEnumerable<string> to, string subject, string body, bool isHtml = true);
}

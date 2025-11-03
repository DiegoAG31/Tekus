using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;

namespace Tekus.Core.Application.Providers.Commands.CreateProvider;

/// <summary>
/// Command to create a new provider
/// </summary>
public class CreateProviderCommand : IRequest<Result<Guid>>
{
    public string Nit { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<CustomFieldDto>? CustomFields { get; set; }
}
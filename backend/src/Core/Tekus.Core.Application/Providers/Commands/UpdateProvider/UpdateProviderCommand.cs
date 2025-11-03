using MediatR;
using Tekus.Core.Application.Common.Models;

namespace Tekus.Core.Application.Providers.Commands.UpdateProvider;

public class UpdateProviderCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
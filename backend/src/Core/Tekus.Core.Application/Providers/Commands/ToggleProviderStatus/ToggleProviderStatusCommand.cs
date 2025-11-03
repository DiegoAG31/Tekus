using MediatR;
using Tekus.Core.Application.Common.Models;

namespace Tekus.Core.Application.Providers.Commands.ToggleProviderStatus;

public class ToggleProviderStatusCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
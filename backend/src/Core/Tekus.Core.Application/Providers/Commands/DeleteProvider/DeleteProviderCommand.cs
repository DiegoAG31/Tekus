using MediatR;
using Tekus.Core.Application.Common.Models;

namespace Tekus.Core.Application.Providers.Commands.DeleteProvider;

public class DeleteProviderCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
using MediatR;
using Tekus.Core.Application.Common.Models;

namespace Tekus.Core.Application.Services.Commands.DeleteService;

public class DeleteServiceCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}

using MediatR;
using Tekus.Core.Application.Common.Models;

namespace Tekus.Core.Application.Services.Commands.AssignCountriesToService;

public class AssignCountriesToServiceCommand : IRequest<Result<bool>>
{
    public Guid ServiceId { get; set; }
    public List<string> CountryCodes { get; set; } = new();
}

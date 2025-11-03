using MediatR;
using Tekus.Core.Application.Common.Models;

namespace Tekus.Core.Application.Services.Commands.UpdateService;

public class UpdateServiceCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public string Currency { get; set; } = "USD";
}

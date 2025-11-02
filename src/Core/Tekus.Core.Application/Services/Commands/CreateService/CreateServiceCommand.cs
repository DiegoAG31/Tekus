using MediatR;
using Tekus.Core.Application.Common.Models;

namespace Tekus.Core.Application.Services.Commands.CreateService;

public class CreateServiceCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public string Currency { get; set; } = "USD";
    public Guid ProviderId { get; set; }
    public List<string>? CountryCodes { get; set; }
}

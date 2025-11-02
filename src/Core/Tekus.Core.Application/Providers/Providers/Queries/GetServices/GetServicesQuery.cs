using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Domain.Common;

namespace Tekus.Core.Application.Services.Queries.GetServices;

public class GetServicesQuery : IRequest<Result<PagedResult<ServiceDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public Guid? ProviderId { get; set; }
    public string? CountryCode { get; set; }
}

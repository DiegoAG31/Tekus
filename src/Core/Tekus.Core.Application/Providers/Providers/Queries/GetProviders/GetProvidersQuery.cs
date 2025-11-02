using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Domain.Common;

namespace Tekus.Core.Application.Providers.Queries.GetProviders;

public class GetProvidersQuery : IRequest<Result<PagedResult<ProviderDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public bool? IsActive { get; set; }
}

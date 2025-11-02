using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;

namespace Tekus.Core.Application.Providers.Queries.GetProviderByNit;

public class GetProviderByNitQuery : IRequest<Result<ProviderDto>>
{
    public string Nit { get; set; } = string.Empty;

    public GetProviderByNitQuery(string nit)
    {
        Nit = nit;
    }
}

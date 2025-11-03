using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;

namespace Tekus.Core.Application.Providers.Queries.GetProviderById;

public class GetProviderByIdQuery : IRequest<Result<ProviderDto>>
{
    public Guid Id { get; set; }

    public GetProviderByIdQuery(Guid id)
    {
        Id = id;
    }
}

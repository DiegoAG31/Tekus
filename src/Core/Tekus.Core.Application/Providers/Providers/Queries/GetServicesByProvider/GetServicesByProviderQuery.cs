using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;

namespace Tekus.Core.Application.Services.Queries.GetServicesByProvider;

public class GetServicesByProviderQuery : IRequest<Result<List<ServiceDto>>>
{
    public Guid ProviderId { get; set; }

    public GetServicesByProviderQuery(Guid providerId)
    {
        ProviderId = providerId;
    }
}

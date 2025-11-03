using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;

namespace Tekus.Core.Application.Services.Queries.GetServiceById;

public class GetServiceByIdQuery : IRequest<Result<ServiceDto>>
{
    public Guid Id { get; set; }

    public GetServiceByIdQuery(Guid id)
    {
        Id = id;
    }
}

using AutoMapper;
using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Domain.Repositories;

namespace Tekus.Core.Application.Providers.Queries.GetProviderById;

public class GetProviderByIdQueryHandler : IRequestHandler<GetProviderByIdQuery, Result<ProviderDto>>
{
    private readonly IProviderRepository _providerRepository;
    private readonly IMapper _mapper;

    public GetProviderByIdQueryHandler(
        IProviderRepository providerRepository,
        IMapper mapper)
    {
        _providerRepository = providerRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProviderDto>> Handle(GetProviderByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var provider = await _providerRepository.GetByIdWithCustomFieldsAsync(request.Id, cancellationToken);

            if (provider == null)
            {
                return Result<ProviderDto>.NotFound("Provider", request.Id);
            }

            var dto = _mapper.Map<ProviderDto>(provider);
            return Result<ProviderDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<ProviderDto>.Failure($"Failed to retrieve provider: {ex.Message}", "GET_PROVIDER_ERROR");
        }
    }
}

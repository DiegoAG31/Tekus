using AutoMapper;
using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Domain.Repositories;

namespace Tekus.Core.Application.Providers.Queries.GetProviderByNit;

public class GetProviderByNitQueryHandler : IRequestHandler<GetProviderByNitQuery, Result<ProviderDto>>
{
    private readonly IProviderRepository _providerRepository;
    private readonly IMapper _mapper;

    public GetProviderByNitQueryHandler(
        IProviderRepository providerRepository,
        IMapper mapper)
    {
        _providerRepository = providerRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProviderDto>> Handle(GetProviderByNitQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var provider = await _providerRepository.GetByNitAsync(request.Nit, cancellationToken);

            if (provider == null)
            {
                return Result<ProviderDto>.Failure($"Provider with NIT '{request.Nit}' not found", "PROVIDER_NOT_FOUND");
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

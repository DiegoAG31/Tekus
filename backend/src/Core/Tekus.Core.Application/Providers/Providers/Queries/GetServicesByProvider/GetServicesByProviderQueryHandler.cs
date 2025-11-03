using AutoMapper;
using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Domain.Repositories;

namespace Tekus.Core.Application.Services.Queries.GetServicesByProvider;

public class GetServicesByProviderQueryHandler : IRequestHandler<GetServicesByProviderQuery, Result<List<ServiceDto>>>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly IMapper _mapper;

    public GetServicesByProviderQueryHandler(
        IServiceRepository serviceRepository,
        IProviderRepository providerRepository,
        IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _providerRepository = providerRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<ServiceDto>>> Handle(GetServicesByProviderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify provider exists
            var providerExists = await _providerRepository.AnyAsync(
                p => p.Id == request.ProviderId,
                cancellationToken);

            if (!providerExists)
            {
                return Result<List<ServiceDto>>.NotFound("Provider", request.ProviderId);
            }

            var services = await _serviceRepository.GetByProviderIdAsync(request.ProviderId, cancellationToken);
            var dtos = _mapper.Map<List<ServiceDto>>(services);

            // Get provider name
            var provider = await _providerRepository.GetByIdAsync(request.ProviderId, cancellationToken);
            if (provider != null)
            {
                foreach (var dto in dtos)
                {
                    dto.ProviderName = provider.Name;
                }
            }

            return Result<List<ServiceDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result<List<ServiceDto>>.Failure($"Failed to retrieve services: {ex.Message}", "GET_SERVICES_ERROR");
        }
    }
}

using AutoMapper;
using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Domain.Repositories;

namespace Tekus.Core.Application.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, Result<ServiceDto>>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly IMapper _mapper;

    public GetServiceByIdQueryHandler(
        IServiceRepository serviceRepository,
        IProviderRepository providerRepository,
        IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _providerRepository = providerRepository;
        _mapper = mapper;
    }

    public async Task<Result<ServiceDto>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var service = await _serviceRepository.GetByIdWithCountriesAsync(request.Id, cancellationToken);

            if (service == null)
            {
                return Result<ServiceDto>.NotFound("Service", request.Id);
            }

            var dto = _mapper.Map<ServiceDto>(service);

            // Get provider name
            var provider = await _providerRepository.GetByIdAsync(service.ProviderId, cancellationToken);
            if (provider != null)
            {
                dto.ProviderName = provider.Name;
            }

            return Result<ServiceDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<ServiceDto>.Failure($"Failed to retrieve service: {ex.Message}", "GET_SERVICE_ERROR");
        }
    }
}

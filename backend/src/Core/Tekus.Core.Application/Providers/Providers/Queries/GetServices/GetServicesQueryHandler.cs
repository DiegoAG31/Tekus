using AutoMapper;
using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Domain.Common;
using Tekus.Core.Domain.Repositories;

namespace Tekus.Core.Application.Services.Queries.GetServices;

public class GetServicesQueryHandler : IRequestHandler<GetServicesQuery, Result<PagedResult<ServiceDto>>>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly IMapper _mapper;

    public GetServicesQueryHandler(
        IServiceRepository serviceRepository,
        IProviderRepository providerRepository,
        IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _providerRepository = providerRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<ServiceDto>>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Domain.Aggregates.ServiceAggregate.Service> services;

            // Filter by specific criteria
            if (request.ProviderId.HasValue)
            {
                services = await _serviceRepository.GetByProviderIdAsync(request.ProviderId.Value, cancellationToken);
            }
            else if (!string.IsNullOrEmpty(request.CountryCode))
            {
                services = await _serviceRepository.GetByCountryCodeAsync(request.CountryCode, cancellationToken);
            }
            else
            {
                services = await _serviceRepository.GetAllAsync(cancellationToken);
            }

            // Apply search term if provided
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                services = services.Where(s => s.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = services.Count();

            // Apply pagination
            var pagedServices = services
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = _mapper.Map<List<ServiceDto>>(pagedServices);

            // Enrich with provider names
            var providerIds = dtos.Select(d => d.ProviderId).Distinct();
            foreach (var providerId in providerIds)
            {
                var provider = await _providerRepository.GetByIdAsync(providerId, cancellationToken);
                if (provider != null)
                {
                    foreach (var dto in dtos.Where(d => d.ProviderId == providerId))
                    {
                        dto.ProviderName = provider.Name;
                    }
                }
            }

            var pagedResult = new PagedResult<ServiceDto>(dtos, totalCount, request.PageNumber, request.PageSize);

            return Result<PagedResult<ServiceDto>>.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<ServiceDto>>.Failure($"Failed to retrieve services: {ex.Message}", "GET_SERVICES_ERROR");
        }
    }
}

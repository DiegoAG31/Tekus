using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Domain.Aggregates.ServiceAggregate;
using Tekus.Core.Domain.Repositories;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.Core.Application.Services.Commands.CreateService;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, Result<Guid>>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceCommandHandler(
        IServiceRepository serviceRepository,
        IProviderRepository providerRepository,
        ICountryRepository countryRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _providerRepository = providerRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify provider exists
            var providerExists = await _providerRepository.AnyAsync(
                p => p.Id == request.ProviderId,
                cancellationToken);

            if (!providerExists)
            {
                return Result<Guid>.Failure($"Provider with ID '{request.ProviderId}' not found", "PROVIDER_NOT_FOUND");
            }

            // Create service
            var service = Service.Create(
                request.ProviderId,
                request.Name,
                Money.Create(request.HourlyRate, request.Currency)
            );

            // Assign countries if provided
            if (request.CountryCodes != null && request.CountryCodes.Any())
            {
                foreach (var countryCode in request.CountryCodes)
                {
                    // Verify country exists
                    var countryExists = await _countryRepository.ExistsAsync(
                        countryCode,
                        cancellationToken);

                    if (!countryExists)
                    {
                        return Result<Guid>.Failure($"Country with code '{countryCode}' not found", "COUNTRY_NOT_FOUND");
                    }

                    service.AssignCountry(countryCode);
                }
            }

            await _serviceRepository.AddAsync(service, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(service.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure($"Failed to create service: {ex.Message}", "CREATE_SERVICE_ERROR");
        }
    }
}

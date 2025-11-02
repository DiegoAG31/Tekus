using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Domain.Repositories;

namespace Tekus.Core.Application.Services.Commands.AssignCountriesToService;

public class AssignCountriesToServiceCommandHandler : IRequestHandler<AssignCountriesToServiceCommand, Result<bool>>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignCountriesToServiceCommandHandler(
        IServiceRepository serviceRepository,
        ICountryRepository countryRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(AssignCountriesToServiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var service = await _serviceRepository.GetByIdWithCountriesAsync(request.ServiceId, cancellationToken);

            if (service == null)
            {
                return Result<bool>.NotFound("Service", request.ServiceId);
            }

            // Clear existing countries
            var existingCountryCodes = service.GetCountryCodes().ToList();
            foreach (var code in existingCountryCodes)
            {
                service.RemoveCountry(code);
            }

            // Assign new countries
            foreach (var countryCode in request.CountryCodes)
            {
                var countryExists = await _countryRepository.ExistsAsync(
                    countryCode,
                    cancellationToken);

                if (!countryExists)
                {
                    return Result<bool>.Failure($"Country with code '{countryCode}' not found", "COUNTRY_NOT_FOUND");
                }

                service.AssignCountry(countryCode);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to assign countries: {ex.Message}", "ASSIGN_COUNTRIES_ERROR");
        }
    }
}

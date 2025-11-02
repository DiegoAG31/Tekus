using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Domain.Repositories;

namespace Tekus.Core.Application.Providers.Commands.DeleteProvider;

public class DeleteProviderCommandHandler : IRequestHandler<DeleteProviderCommand, Result<bool>>
{
    private readonly IProviderRepository _providerRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProviderCommandHandler(
        IProviderRepository providerRepository,
        IServiceRepository serviceRepository,
        IUnitOfWork unitOfWork)
    {
        _providerRepository = providerRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteProviderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var provider = await _providerRepository.GetByIdAsync(request.Id, cancellationToken);

            if (provider == null)
            {
                return Result<bool>.NotFound("Provider", request.Id);
            }

            // Check if provider has services
            var hasServices = await _serviceRepository.AnyAsync(
                s => s.ProviderId == request.Id,
                cancellationToken);

            if (hasServices)
            {
                return Result<bool>.Failure(
                    "Cannot delete provider with existing services",
                    "PROVIDER_HAS_SERVICES");
            }

            await _providerRepository.DeleteAsync(provider, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to delete provider: {ex.Message}", "DELETE_PROVIDER_ERROR");
        }
    }
}
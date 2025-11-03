using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Domain.Repositories;

namespace Tekus.Core.Application.Providers.Commands.ToggleProviderStatus;

public class ToggleProviderStatusCommandHandler : IRequestHandler<ToggleProviderStatusCommand, Result<bool>>
{
    private readonly IProviderRepository _providerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ToggleProviderStatusCommandHandler(
        IProviderRepository providerRepository,
        IUnitOfWork unitOfWork)
    {
        _providerRepository = providerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(ToggleProviderStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var provider = await _providerRepository.GetByIdAsync(request.Id, cancellationToken);

            if (provider == null)
            {
                return Result<bool>.NotFound("Provider", request.Id);
            }

            if (provider.IsActive)
            {
                provider.Deactivate();
            }
            else
            {
                provider.Activate();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to toggle provider status: {ex.Message}", "TOGGLE_STATUS_ERROR");
        }
    }
}
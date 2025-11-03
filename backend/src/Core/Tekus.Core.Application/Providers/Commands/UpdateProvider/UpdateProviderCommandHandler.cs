using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Domain.Repositories;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.Core.Application.Providers.Commands.UpdateProvider;

public class UpdateProviderCommandHandler : IRequestHandler<UpdateProviderCommand, Result<bool>>
{
    private readonly IProviderRepository _providerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProviderCommandHandler(
        IProviderRepository providerRepository,
        IUnitOfWork unitOfWork)
    {
        _providerRepository = providerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(UpdateProviderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var provider = await _providerRepository.GetByIdAsync(request.Id, cancellationToken);

            if (provider == null)
            {
                return Result<bool>.NotFound("Provider", request.Id);
            }

            provider.Update(request.Name, Email.Create(request.Email));

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to update provider: {ex.Message}", "UPDATE_PROVIDER_ERROR");
        }
    }
}
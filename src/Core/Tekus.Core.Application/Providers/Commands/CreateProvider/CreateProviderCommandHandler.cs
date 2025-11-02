using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;
using Tekus.Core.Domain.Repositories;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.Core.Application.Providers.Commands.CreateProvider;

/// <summary>
/// Handler for CreateProviderCommand
/// </summary>
public class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand, Result<Guid>>
{
    private readonly IProviderRepository _providerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProviderCommandHandler(
        IProviderRepository providerRepository,
        IUnitOfWork unitOfWork)
    {
        _providerRepository = providerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if NIT already exists
            var nitExists = await _providerRepository.AnyAsync(
                p => p.Nit == Nit.Create(request.Nit),
                cancellationToken);

            if (nitExists)
            {
                return Result<Guid>.Failure(
                    $"Provider with NIT '{request.Nit}' already exists",
                    "DUPLICATE_NIT");
            }

            // Create Provider
            var provider = Provider.Create(
                Nit.Create(request.Nit),
                request.Name,
                Email.Create(request.Email)
            );

            // Add custom fields if provided
            if (request.CustomFields != null)
            {
                foreach (var field in request.CustomFields)
                {
                    provider.AddCustomField(field.Key, field.Value, field.Type);
                }
            }

            // Save to repository
            await _providerRepository.AddAsync(provider, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(provider.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(
                $"Failed to create provider: {ex.Message}",
                "CREATE_PROVIDER_ERROR");
        }
    }
}
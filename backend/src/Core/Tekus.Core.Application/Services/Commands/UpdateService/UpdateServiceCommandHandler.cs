using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Domain.Repositories;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.Core.Application.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, Result<bool>>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateServiceCommandHandler(
        IServiceRepository serviceRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var service = await _serviceRepository.GetByIdAsync(request.Id, cancellationToken);

            if (service == null)
            {
                return Result<bool>.NotFound("Service", request.Id);
            }

            service.Update(request.Name, Money.Create(request.HourlyRate, request.Currency));

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to update service: {ex.Message}", "UPDATE_SERVICE_ERROR");
        }
    }
}

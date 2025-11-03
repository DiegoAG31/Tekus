using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Domain.Repositories;

namespace Tekus.Core.Application.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, Result<bool>>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteServiceCommandHandler(
        IServiceRepository serviceRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var service = await _serviceRepository.GetByIdAsync(request.Id, cancellationToken);

            if (service == null)
            {
                return Result<bool>.NotFound("Service", request.Id);
            }

            await _serviceRepository.DeleteAsync(service, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to delete service: {ex.Message}", "DELETE_SERVICE_ERROR");
        }
    }
}

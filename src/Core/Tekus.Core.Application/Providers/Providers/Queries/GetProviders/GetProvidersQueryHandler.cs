using AutoMapper;
using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Domain.Common;
using Tekus.Core.Domain.Repositories;

namespace Tekus.Core.Application.Providers.Queries.GetProviders;

public class GetProvidersQueryHandler : IRequestHandler<GetProvidersQuery, Result<PagedResult<ProviderDto>>>
{
    private readonly IProviderRepository _providerRepository;
    private readonly IMapper _mapper;

    public GetProvidersQueryHandler(
        IProviderRepository providerRepository,
        IMapper mapper)
    {
        _providerRepository = providerRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<ProviderDto>>> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedResult = await _providerRepository.GetPagedAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                searchTerm: request.SearchTerm,
                cancellationToken: cancellationToken);

            var providers = pagedResult.Items;
            var totalCount = pagedResult.TotalCount;

            var dtos = _mapper.Map<List<ProviderDto>>(providers);
            var result = new PagedResult<ProviderDto>(dtos, totalCount, request.PageNumber, request.PageSize);

            return Result<PagedResult<ProviderDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<ProviderDto>>.Failure($"Failed to retrieve providers: {ex.Message}", "GET_PROVIDERS_ERROR");
        }
    }
}

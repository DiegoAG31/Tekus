using AutoMapper;
using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Domain.Repositories;

namespace Tekus.Core.Application.Countries.Queries.GetCountryByCode;

public class GetCountryByCodeQueryHandler : IRequestHandler<GetCountryByCodeQuery, Result<CountryDto>>
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public GetCountryByCodeQueryHandler(
        ICountryRepository countryRepository,
        IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    public async Task<Result<CountryDto>> Handle(GetCountryByCodeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var country = await _countryRepository.GetByCodeAsync(request.Code, cancellationToken);

            if (country == null)
            {
                return Result<CountryDto>.Failure($"Country with code '{request.Code}' not found", "COUNTRY_NOT_FOUND");
            }

            var dto = _mapper.Map<CountryDto>(country);
            return Result<CountryDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<CountryDto>.Failure($"Failed to retrieve country: {ex.Message}", "GET_COUNTRY_ERROR");
        }
    }
}
using AutoMapper;
using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Domain.Repositories;

namespace Tekus.Core.Application.Countries.Queries.GetCountries;

public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, Result<List<CountryDto>>>
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public GetCountriesQueryHandler(
        ICountryRepository countryRepository,
        IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<CountryDto>>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var countries = await _countryRepository.GetAllAsync(cancellationToken);
            var dtos = _mapper.Map<List<CountryDto>>(countries);

            return Result<List<CountryDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result<List<CountryDto>>.Failure($"Failed to retrieve countries: {ex.Message}", "GET_COUNTRIES_ERROR");
        }
    }
}
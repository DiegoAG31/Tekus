using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;

namespace Tekus.Core.Application.Countries.Queries.GetCountries;

public class GetCountriesQuery : IRequest<Result<List<CountryDto>>>
{
}
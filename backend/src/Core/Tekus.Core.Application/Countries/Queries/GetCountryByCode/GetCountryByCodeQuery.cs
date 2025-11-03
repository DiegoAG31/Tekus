using MediatR;
using Tekus.Core.Application.Common.Models;
using Tekus.Core.Application.DTOs;

namespace Tekus.Core.Application.Countries.Queries.GetCountryByCode;

public class GetCountryByCodeQuery : IRequest<Result<CountryDto>>
{
    public string Code { get; set; } = string.Empty;

    public GetCountryByCodeQuery(string code)
    {
        Code = code;
    }
}
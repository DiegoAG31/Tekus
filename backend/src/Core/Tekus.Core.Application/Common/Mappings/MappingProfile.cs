using AutoMapper;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;
using Tekus.Core.Domain.Aggregates.ServiceAggregate;
using Tekus.Core.Domain.Entities;

namespace Tekus.Core.Application.Common.Mappings;

/// <summary>
/// AutoMapper profile for mapping between Domain entities and DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Provider mappings
        CreateMap<Provider, ProviderDto>()
            .ForMember(dest => dest.Nit, opt => opt.MapFrom(src => src.Nit.Value))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.CustomFields, opt => opt.MapFrom(src => src.CustomFields));

        CreateMap<ProviderCustomField, CustomFieldDto>()
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.FieldName))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.FieldValue))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.FieldType));

        // Service mappings
        CreateMap<Service, ServiceDto>()
            .ForMember(dest => dest.HourlyRate, opt => opt.MapFrom(src => src.HourlyRate.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.HourlyRate.Currency))
            .ForMember(dest => dest.CountryCodes, opt => opt.MapFrom(src => src.GetCountryCodes()))
            .ForMember(dest => dest.ProviderName, opt => opt.Ignore()); // Will be populated from query

        // Country mappings
        CreateMap<Country, CountryDto>();
    }
}
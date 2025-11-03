using AutoMapper;
using Tekus.Core.Application.Common.Mappings;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;
using Tekus.Core.Domain.Aggregates.ServiceAggregate;
using Tekus.Core.Domain.Entities;
using Tekus.Core.Domain.ValueObjects;
using Xunit;

namespace Tekus.UnitTests.Application.Mappings;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void MappingProfile_ShouldBeValid()
    {
        // Act & Assert - AutoMapper will throw if configuration is invalid
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_Provider_To_ProviderDto_ShouldMapCorrectly()
    {
        // Arrange
        var provider = Provider.Create(
            Nit.Create("900123456"),
            "Tekus SAS",
            Email.Create("info@tekus.com")
        );
        var fieldName = "Address";
        var fieldValue = "+1-234-567-8900";
        var fieldType = "text";

        // Act
        provider.AddCustomField(fieldName, fieldValue, fieldType);
        provider.AddCustomField("Phone", "+57 300 1234567","text");


        // Act
        var dto = _mapper.Map<ProviderDto>(provider);

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(provider.Id, dto.Id);
        Assert.Equal(provider.Nit.Value, dto.Nit);
        Assert.Equal(provider.Name, dto.Name);
        Assert.Equal(provider.Email.Value, dto.Email);
        Assert.Equal(provider.IsActive, dto.IsActive);
        Assert.Equal(provider.CreatedAt, dto.CreatedAt);
        Assert.Equal(provider.UpdatedAt, dto.UpdatedAt);
        Assert.Equal(2, dto.CustomFields.Count);
        Assert.Contains(dto.CustomFields, cf => cf.Key == "Phone");
        Assert.Contains(dto.CustomFields, cf => cf.Key == "Address");
    }

    [Fact]
    public void Map_Service_To_ServiceDto_ShouldMapCorrectly()
    {
        // Arrange
        var providerId = Guid.NewGuid();
        var service = Service.Create(
            providerId: providerId,
            "Cloud Hosting",
            Money.Create(150.50m, "USD")
        );

        service.AssignCountry("COL");
        service.AssignCountry("PER");

        // Act
        var dto = _mapper.Map<ServiceDto>(service);

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(service.Id, dto.Id);
        Assert.Equal(service.Name, dto.Name);
        Assert.Equal(service.HourlyRate.Amount, dto.HourlyRate);
        Assert.Equal(service.HourlyRate.Currency, dto.Currency);
        Assert.Equal(service.ProviderId, dto.ProviderId);
        Assert.Equal(2, dto.CountryCodes.Count);
        Assert.Contains("COL", dto.CountryCodes);
        Assert.Contains("PER", dto.CountryCodes);
    }

    [Fact]
    public void Map_Country_To_CountryDto_ShouldMapCorrectly()
    {
        // Arrange
        var country = Country.Create("COL", "Colombia");
        country.UpdateSync();

        // Act
        var dto = _mapper.Map<CountryDto>(country);

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(country.Code, dto.Code);
        Assert.Equal(country.Name, dto.Name);
        Assert.Equal(country.LastSync, dto.LastSync);
    }

    [Fact]
    public void Map_ProviderList_To_ProviderDtoList_ShouldMapCorrectly()
    {
        // Arrange
        var providers = new List<Provider>
        {
            Provider.Create(Nit.Create("900111111"), "Provider 1", Email.Create("p1@test.com")),
            Provider.Create(Nit.Create("900222222"), "Provider 2", Email.Create("p2@test.com")),
            Provider.Create(Nit.Create("900333333"), "Provider 3", Email.Create("p3@test.com"))
        };

        // Act
        var dtos = _mapper.Map<List<ProviderDto>>(providers);

        // Assert
        Assert.NotNull(dtos);
        Assert.Equal(3, dtos.Count);
        Assert.Equal("Provider 1", dtos[0].Name);
        Assert.Equal("Provider 2", dtos[1].Name);
        Assert.Equal("Provider 3", dtos[2].Name);
    }
}
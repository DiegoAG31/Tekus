using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;
using Tekus.Core.Domain.Repositories;
using Tekus.Core.Domain.ValueObjects;
using Tekus.Infrastructure.Persistence;
using Tekus.Infrastructure.Persistence.Data;
using Xunit;

namespace Tekus.IntegrationTests.Repositories;

public class ProviderRepositoryTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ApplicationDbContext _context;
    private readonly IProviderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProviderRepositoryTests()
    {
        var services = new ServiceCollection();

        // Add Infrastructure services with InMemory database
        services.AddInfrastructureServicesInMemory();

        _serviceProvider = services.BuildServiceProvider();
        _context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
        _repository = _serviceProvider.GetRequiredService<IProviderRepository>();
        _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddProviderToDatabase()
    {
        // Arrange
        var provider = Provider.Create(
            Nit.Create("900123456"),
            "Tekus SAS",
            Email.Create("info@tekus.com")
        );

        // Act
        await _repository.AddAsync(provider);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var saved = await _context.Providers.FindAsync(provider.Id);
        saved.Should().NotBeNull();
        saved!.Nit.Value.Should().Be("900123456");
        saved.Name.Should().Be("Tekus SAS");
        saved.Email.Value.Should().Be("info@tekus.com");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProvider()
    {
        // Arrange
        var provider = Provider.Create(
            Nit.Create("900111111"),
            "Provider 1",
            Email.Create("p1@test.com")
        );

        await _repository.AddAsync(provider);
        await _unitOfWork.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(provider.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(provider.Id);
        result.Name.Should().Be("Provider 1");
    }

    [Fact]
    public async Task GetByNitAsync_ShouldReturnProviderWithCustomFields()
    {
        // Arrange
        var provider = Provider.Create(
            Nit.Create("900222222"),
            "Provider 2",
            Email.Create("p2@test.com")
        );
        provider.AddCustomField("Phone", "+57 300 1234567", "string");
        provider.AddCustomField("Address", "Calle 123", "string");

        await _repository.AddAsync(provider);
        await _unitOfWork.SaveChangesAsync();

        // Detach to simulate new request
        _context.Entry(provider).State = EntityState.Detached;

        // Act
        var result = await _repository.GetByNitAsync("900222222");

        // Assert
        result.Should().NotBeNull();
        result!.CustomFields.Should().HaveCount(2);
        result.CustomFields.Should().Contain(cf => cf.FieldName == "Phone");
        result.CustomFields.Should().Contain(cf => cf.FieldName == "Address");
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnPagedResults()
    {
        // Arrange
        for (int i = 1; i <= 15; i++)
        {
            var provider = Provider.Create(
                Nit.Create($"90000000{i:00}"),
                $"Provider {i}",
                Email.Create($"provider{i}@test.com")
            );
            await _repository.AddAsync(provider);
        }
        await _unitOfWork.SaveChangesAsync();

        // Act
        var (items, totalCount) = await _repository.GetPagedAsync(
            pageNumber: 2,
            pageSize: 5
        );

        // Assert
        totalCount.Should().Be(15);
        items.Should().HaveCount(5);
    }

    [Fact]
    public async Task GetPagedAsync_WithFilters_ShouldReturnFilteredResults()
    {
        // Arrange
        var provider1 = Provider.Create(
            Nit.Create("900333333"),
            "Tekus Colombia",
            Email.Create("colombia@tekus.com")
        );

        var provider2 = Provider.Create(
            Nit.Create("900444444"),
            "Tekus Peru",
            Email.Create("peru@tekus.com")
        );
        provider2.Deactivate();

        var provider3 = Provider.Create(
            Nit.Create("900555555"),
            "Other Company",
            Email.Create("other@company.com")
        );

        await _repository.AddAsync(provider1);
        await _repository.AddAsync(provider2);
        await _repository.AddAsync(provider3);
        await _unitOfWork.SaveChangesAsync();

        // Act
        var (items, totalCount) = await _repository.GetPagedAsync(
            pageNumber: 1,
            pageSize: 10,
            searchTerm: "Tekus",
            isActive: true
        );

        // Assert
        totalCount.Should().Be(1);
        items.Should().HaveCount(1);
        items.First().Name.Should().Be("Tekus Colombia");
    }

    [Fact]
    public async Task Update_ShouldUpdateProviderInDatabase()
    {
        // Arrange
        var provider = Provider.Create(
            Nit.Create("900666666"),
            "Original Name",
            Email.Create("original@test.com")
        );

        await _repository.AddAsync(provider);
        await _unitOfWork.SaveChangesAsync();

        // Act
        provider.Update("Updated Name", Email.Create("updated@test.com"));
        await _repository.UpdateAsync(provider);
        await _unitOfWork.SaveChangesAsync();

        // Detach and re-query
        _context.Entry(provider).State = EntityState.Detached;
        var updated = await _repository.GetByIdAsync(provider.Id);

        // Assert
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Updated Name");
        updated.Email.Value.Should().Be("updated@test.com");
    }

    [Fact]
    public async Task Delete_ShouldRemoveProviderFromDatabase()
    {
        // Arrange
        var provider = Provider.Create(
            Nit.Create("900777777"),
            "To Be Deleted",
            Email.Create("delete@test.com")
        );

        await _repository.AddAsync(provider);
        await _unitOfWork.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(provider);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var deleted = await _repository.GetByIdAsync(provider.Id);
        deleted.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        _serviceProvider.Dispose();
    }
}
using FluentValidation.TestHelper;
using Moq;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Application.Providers.Commands.CreateProvider;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;
using Tekus.Core.Domain.Repositories;
using Tekus.Core.Domain.ValueObjects;
using Xunit;

namespace Tekus.UnitTests.Application.Providers.Commands;

public class CreateProviderCommandTests
{
    private readonly CreateProviderCommandValidator _validator;

    public CreateProviderCommandTests()
    {
        _validator = new CreateProviderCommandValidator();
    }

    #region Validator Tests

    [Fact]
    public void Validator_WithValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = "900123456",
            Name = "Tekus SAS",
            Email = "info@tekus.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_WithEmptyNit_ShouldHaveError()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = "",
            Name = "Tekus SAS",
            Email = "info@tekus.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nit);
    }

    [Fact]
    public void Validator_WithNitTooLong_ShouldHaveError()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = new string('1', 21), // 21 characters
            Name = "Tekus SAS",
            Email = "info@tekus.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nit);
    }

    [Fact]
    public void Validator_WithEmptyName_ShouldHaveError()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = "900123456",
            Name = "",
            Email = "info@tekus.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validator_WithNameTooLong_ShouldHaveError()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = "900123456",
            Name = new string('A', 201), // 201 characters
            Email = "info@tekus.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validator_WithInvalidEmail_ShouldHaveError()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = "900123456",
            Name = "Tekus SAS",
            Email = "invalid-email"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    #endregion

    #region Handler Tests

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateProvider()
    {
        // Arrange
        var mockProviderRepo = new Mock<IProviderRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockProviderRepo.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Provider, bool>>>(), default))
            .ReturnsAsync(false);

        Provider? capturedProvider = null;
        mockProviderRepo.Setup(x => x.AddAsync(It.IsAny<Provider>(), default))
            .Callback<Provider, System.Threading.CancellationToken>((p, ct) => capturedProvider = p)
            .ReturnsAsync((Provider?)null);

        mockUnitOfWork.Setup(x => x.SaveChangesAsync(default))
            .ReturnsAsync(1);

        var handler = new CreateProviderCommandHandler(mockProviderRepo.Object, mockUnitOfWork.Object);

        var command = new CreateProviderCommand
        {
            Nit = "900123456",
            Name = "Tekus SAS",
            Email = "info@tekus.com"
        };

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);

        Assert.NotNull(capturedProvider);
        Assert.Equal("900123456", capturedProvider.Nit.Value);
        Assert.Equal("Tekus SAS", capturedProvider.Name);
        Assert.Equal("info@tekus.com", capturedProvider.Email.Value);

        mockProviderRepo.Verify(x => x.AddAsync(It.IsAny<Provider>(), default), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_WithDuplicateNit_ShouldReturnFailure()
    {
        // Arrange
        var mockProviderRepo = new Mock<IProviderRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockProviderRepo.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Provider, bool>>>(), default))
            .ReturnsAsync(true); // NIT already exists

        var handler = new CreateProviderCommandHandler(mockProviderRepo.Object, mockUnitOfWork.Object);

        var command = new CreateProviderCommand
        {
            Nit = "900123456",
            Name = "Tekus SAS",
            Email = "info@tekus.com"
        };

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("already exists", result.Error);

        mockProviderRepo.Verify(x => x.AddAsync(It.IsAny<Provider>(), default), Times.Never);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task Handle_WithCustomFields_ShouldCreateProviderWithCustomFields()
    {
        // Arrange
        var mockProviderRepo = new Mock<IProviderRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockProviderRepo.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Provider, bool>>>(), default))
            .ReturnsAsync(false);

        Provider? capturedProvider = null;
        mockProviderRepo.Setup(x => x.AddAsync(It.IsAny<Provider>(), default))
            .Callback<Provider, System.Threading.CancellationToken>((p, ct) => capturedProvider = p)
            .ReturnsAsync((Provider?)null);

        mockUnitOfWork.Setup(x => x.SaveChangesAsync(default))
            .ReturnsAsync(1);

        var handler = new CreateProviderCommandHandler(mockProviderRepo.Object, mockUnitOfWork.Object);

        var command = new CreateProviderCommand
        {
            Nit = "900123456",
            Name = "Tekus SAS",
            Email = "info@tekus.com",
            CustomFields = new List<CustomFieldDto>
            {
                new CustomFieldDto { Key = "Phone", Value = "+57 300 1234567", Type = "string" },
                new CustomFieldDto { Key = "Address", Value = "Calle 123", Type = "string" }
            }
        };

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedProvider);
        Assert.Equal(2, capturedProvider.CustomFields.Count);
    }

    #endregion
}

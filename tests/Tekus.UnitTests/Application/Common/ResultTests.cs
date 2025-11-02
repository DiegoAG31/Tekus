using Tekus.Core.Application.Common.Models;
using Xunit;

namespace Tekus.UnitTests.Application.Common;

public class ResultTests
{
    [Fact]
    public void Success_WithValue_ShouldReturnSuccessResult()
    {
        // Arrange
        var value = "Test Value";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
        Assert.Null(result.ErrorCode);
    }

    [Fact]
    public void Success_WithoutValue_ShouldReturnSuccessResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
        Assert.Null(result.ErrorCode);
    }

    [Fact]
    public void Failure_WithError_ShouldReturnFailureResult()
    {
        // Arrange
        var error = "Something went wrong";

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
        Assert.Null(result.ErrorCode);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void Failure_WithErrorAndCode_ShouldReturnFailureResult()
    {
        // Arrange
        var error = "Not found";
        var errorCode = "PROVIDER_NOT_FOUND";

        // Act
        var result = Result<string>.Failure(error, errorCode);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
        Assert.Equal(errorCode, result.ErrorCode);
    }

    [Fact]
    public void Failure_WithoutValue_ShouldReturnFailureResult()
    {
        // Arrange
        var error = "Operation failed";

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void NotFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        var entityName = "Provider";
        var entityId = Guid.NewGuid();

        // Act
        var result = Result<string>.NotFound(entityName, entityId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("NOT_FOUND", result.ErrorCode);
        Assert.Contains(entityName, result.Error);
        Assert.Contains(entityId.ToString(), result.Error);
    }

    [Fact]
    public void ValidationError_ShouldReturnValidationErrorResult()
    {
        // Arrange
        var validationErrors = new Dictionary<string, string[]>
        {
            { "Email", new[] { "Email is required", "Email is invalid" } },
            { "Nit", new[] { "Nit is required" } }
        };

        // Act
        var result = Result<string>.ValidationError(validationErrors);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("VALIDATION_ERROR", result.ErrorCode);
        Assert.NotNull(result.ValidationErrors);
        Assert.Equal(2, result.ValidationErrors.Count);
        Assert.Equal(2, result.ValidationErrors["Email"].Length);
    }

    [Fact]
    public void ImplicitOperator_FromValue_ShouldCreateSuccessResult()
    {
        // Arrange
        var value = 42;

        // Act
        Result<int> result = value;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Match_WithSuccess_ShouldExecuteSuccessFunc()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var successCalled = false;
        var failureCalled = false;

        // Act
        var output = result.Match(
            onSuccess: value => { successCalled = true; return value * 2; },
            onFailure: error => { failureCalled = true; return 0; }
        );

        // Assert
        Assert.True(successCalled);
        Assert.False(failureCalled);
        Assert.Equal(84, output);
    }

    [Fact]
    public void Match_WithFailure_ShouldExecuteFailureFunc()
    {
        // Arrange
        var result = Result<int>.Failure("Error");
        var successCalled = false;
        var failureCalled = false;

        // Act
        var output = result.Match(
            onSuccess: value => { successCalled = true; return value * 2; },
            onFailure: error => { failureCalled = true; return -1; }
        );

        // Assert
        Assert.False(successCalled);
        Assert.True(failureCalled);
        Assert.Equal(-1, output);
    }
}

namespace Tekus.Core.Application.Common.Models;

/// <summary>
/// Represents the result of an operation with a value
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; private init; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; private init; }
    public string? ErrorCode { get; private init; }
    public Dictionary<string, string[]>? ValidationErrors { get; private init; }

    private readonly T? _value;

    public T Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException("Cannot access Value of a failed result");
            return _value!;
        }
        private init => _value = value;
    }

    private Result(bool isSuccess, T? value, string? error, string? errorCode, Dictionary<string, string[]>? validationErrors)
    {
        IsSuccess = isSuccess;
        _value = value;
        Error = error;
        ErrorCode = errorCode;
        ValidationErrors = validationErrors;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null, null, null);
    }

    public static Result<T> Failure(string error, string? errorCode = null)
    {
        return new Result<T>(false, default, error, errorCode, null);
    }

    public static Result<T> NotFound(string entityName, object entityId)
    {
        return new Result<T>(
            false,
            default,
            $"{entityName} with id '{entityId}' was not found",
            "NOT_FOUND",
            null
        );
    }

    public static Result<T> ValidationError(Dictionary<string, string[]> validationErrors)
    {
        var errorMessage = "One or more validation errors occurred";
        return new Result<T>(
            false,
            default,
            errorMessage,
            "VALIDATION_ERROR",
            validationErrors
        );
    }

    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<string, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Value) : onFailure(Error!);
    }
}

/// <summary>
/// Represents the result of an operation without a value
/// </summary>
public class Result
{
    public bool IsSuccess { get; private init; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; private init; }
    public string? ErrorCode { get; private init; }

    private Result(bool isSuccess, string? error, string? errorCode = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorCode = errorCode;
    }

    public static Result Success()
    {
        return new Result(true, null);
    }

    public static Result Failure(string error, string? errorCode = null)
    {
        return new Result(false, error, errorCode);
    }

    public static Result NotFound(string entityName, object entityId)
    {
        return new Result(
            false,
            $"{entityName} with id '{entityId}' was not found",
            "NOT_FOUND"
        );
    }
}
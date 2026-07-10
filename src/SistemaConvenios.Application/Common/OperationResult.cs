namespace SistemaConvenios.Application.Common;

public record OperationResult(bool Succeeded, string? Error = null)
{
    public static OperationResult Success() => new(true);
    public static OperationResult Failure(string error) => new(false, error);
}

public sealed record OperationResult<T>(bool Succeeded, T? Value = default, string? Error = null)
{
    public static OperationResult<T> Success(T value) => new(true, value);
    public static OperationResult<T> Failure(string error) => new(false, default, error);
}

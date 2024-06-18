namespace CleanArch.Domain.SeedWork.Models;

public record Result<T, TError>
{
    public static Result<T, TError?> Success(T value) => new(value, default);
    public static Result<T?, TError> Failure(TError error) => new(default, error);

    public T Value { get; }
    public TError Error { get; }
    public bool IsSuccess { get; }

    private Result(T value, TError error)
    {
        Value = value;
        Error = error;
        IsSuccess = value is not null;
    }

    public void Match(Action<T> success, Action<TError> failure)
    {
        if (IsSuccess) success(Value);
        else failure(Error);
    }
}

namespace CleanArch.Domain.SeedWork.Models;

public record Option<T>
{
    public static Option<T?> None => new(default(T));
    public static Option<T> Some(T value) => new(value);

    private T Value { get; }
    private bool IsSome { get; }

    private Option(T value)
    {
        Value = value;
        IsSome = value is not null;
    }

    public T GetValueOrDefault(T defaultValue) => IsSome ? Value : defaultValue;

    public void Match(Action<T> some, Action none)
    {
        if (IsSome) some(Value);
        else none();
    }
}

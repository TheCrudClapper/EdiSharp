namespace EdiSharp.Domain.ResultTypes;

public sealed record Error(string Description)
{
    public static readonly Error None
        = new(string.Empty);

    public static readonly Error NullValue
        = new("NullValue");

    public static Error Create(string message)
        => new(message);
}


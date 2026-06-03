namespace EdiSharp.Core.Exceptions;

public class EdiSemanticsException : Exception
{
    public EdiSemanticsException()
    {
    }

    public EdiSemanticsException(string? message) : base(message)
    {
    }
}

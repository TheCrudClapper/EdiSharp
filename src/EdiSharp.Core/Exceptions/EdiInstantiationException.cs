namespace EdiSharp.Core.Exceptions;

public class EdiInstantiationException : Exception
{
    public EdiInstantiationException()
    {
    }

    public EdiInstantiationException(string? message) : base(message)
    {
    }
}

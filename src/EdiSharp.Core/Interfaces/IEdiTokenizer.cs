using EdiSharp.Core.Enums;

namespace EdiSharp.Core.Interfaces;

public interface IEdiTokenizer
{
    Stream Tokenize(Stream stream, InputType type);
}

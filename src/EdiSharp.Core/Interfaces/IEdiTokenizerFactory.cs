using EdiSharp.Core.Enums;

namespace EdiSharp.Core.Interfaces;

public interface IEdiTokenizerFactory
{
    IEdiTokenizer Create(InputType inputType);
}

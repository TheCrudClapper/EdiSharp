using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;

namespace EdiSharp.Core.Abstractions;

public interface IEdiTokenizerFactory
{
    IEdiTokenizer Create(InputType inputType);
}

using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;

namespace EdiSharp.Core.Factories.Abstractions;

public interface IEdiTokenizerFactory
{
    IEdiTokenizer? TryCreate(EdiStandard inputType);
}

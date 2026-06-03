using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;

namespace EdiSharp.Core.Factories.Abstractions;

public interface IEdiInterchangeBuilderFactory
{
    IEdiInterchangeBuilder? TryCreate(EdiStandard inputType);
}

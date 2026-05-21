using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;

namespace EdiSharp.Core.Factories.Abstractions;

public interface IEdiVersionExtractorFactory
{
    IEdiVersionExtractor? TryCreate(InputType type);
}

using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;

namespace EdiSharp.Core.Factories.Implementations;

public class EdiVersionExtractorFactory : IEdiVersionExtractorFactory
{
    private readonly Dictionary<InputType,  IEdiVersionExtractor> _map;
    public EdiVersionExtractorFactory(IEnumerable<IEdiVersionExtractor> extractors)
    {
        _map = extractors.ToDictionary(e => e.InputType);
    }
    public IEdiVersionExtractor? TryCreate(InputType type)
        => _map.TryGetValue(type, out var extractor) ? extractor : null;
}

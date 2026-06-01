using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;

namespace EdiSharp.Core.Factories.Implementations;

public class EdiMessageSplitterFactory : IEdiInterchangeBuilderFactory
{
    private readonly Dictionary<InputType, IEdiInterchangeBuilder> _map;
    public EdiMessageSplitterFactory(IEnumerable<IEdiInterchangeBuilder> splitters)
        => _map = splitters.ToDictionary(s => s.InputType);

    public IEdiInterchangeBuilder? TryCreate(InputType inputType)
        => _map.TryGetValue(inputType, out var splitter) ? splitter : null;
    
}

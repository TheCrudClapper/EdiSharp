using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;

namespace EdiSharp.Core.Factories.Implementations;

public class EdiMessageSplitterFactory : IEdiMessageSplitterFactory
{
    private readonly Dictionary<InputType, IEdiMessageSplitter> _map;
    public EdiMessageSplitterFactory(IEnumerable<IEdiMessageSplitter> splitters)
        => _map = splitters.ToDictionary(s => s.InputType);

    public IEdiMessageSplitter? TryCreate(InputType inputType)
        => _map.TryGetValue(inputType, out var splitter) ? splitter : null;
    
}

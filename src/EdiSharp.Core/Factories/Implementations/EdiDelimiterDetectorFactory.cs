using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;

namespace EdiSharp.Core.Factories.Implementations;

public class EdiDelimiterDetectorFactory : IEdiDelimiterDetectorFactory
{
    private readonly Dictionary<InputType, IEdiDelimiterDetector> _map;
    public EdiDelimiterDetectorFactory(IEnumerable<IEdiDelimiterDetector> detectors)
    {
        _map = detectors.ToDictionary(d => d.InputType);
    }

    public IEdiDelimiterDetector? TryCreate(InputType inputType)
        => _map.TryGetValue(inputType, out var detector) ? detector : null;
}

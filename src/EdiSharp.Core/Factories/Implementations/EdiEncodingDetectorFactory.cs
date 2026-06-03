using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;
using EdiSharp.Core.Interfaces;

namespace EdiSharp.Core.Factories.Implementations;

public class EdiEncodingDetectorFactory : IEdiEncodingDetectorFactory
{
    private readonly Dictionary<EdiStandard, IEdiEncodingDetector> _map;
    public EdiEncodingDetectorFactory(IEnumerable<IEdiEncodingDetector> detectors)
    {
        _map = detectors.ToDictionary(d => d.InputType);
    }

    public IEdiEncodingDetector? TryCreate(EdiStandard inputType)
        => _map.TryGetValue(inputType, out var detector) ? detector : null;
}

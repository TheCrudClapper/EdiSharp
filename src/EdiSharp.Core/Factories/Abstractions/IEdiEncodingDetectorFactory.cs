using EdiSharp.Core.Enums;
using EdiSharp.Core.Interfaces;

namespace EdiSharp.Core.Factories.Abstractions;

public interface IEdiEncodingDetectorFactory
{
    IEdiEncodingDetector Create(InputType inputType);
}

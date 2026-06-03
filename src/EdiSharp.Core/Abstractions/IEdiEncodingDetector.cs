using EdiSharp.Core.Enums;
using System.Text;

namespace EdiSharp.Core.Interfaces;

public interface IEdiEncodingDetector
{
    EdiStandard InputType { get; }
    Encoding DetermineEncoding(byte[] fileBytes);
}

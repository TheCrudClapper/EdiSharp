using System.Text;

namespace EdiSharp.Core.Interfaces;

public interface IEdiEncodingDetector
{
    Encoding DetermineEncodingType(byte[] fileBytes);
}

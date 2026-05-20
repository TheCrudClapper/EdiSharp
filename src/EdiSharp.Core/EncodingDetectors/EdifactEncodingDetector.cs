using EdiSharp.Core.Interfaces;
using System.Text;

namespace EdiSharp.Core.EncodingDetectors;

public class EdifactEncodingDetector : IEdiEncodingDetector
{
    public Encoding DetermineEncodingType(byte[] fileBytes)
    {
        throw new NotImplementedException();
    }
}

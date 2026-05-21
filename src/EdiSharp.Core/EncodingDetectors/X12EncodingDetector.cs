using EdiSharp.Core.Enums;
using EdiSharp.Core.Interfaces;
using System.Text;

namespace EdiSharp.Core.EncodingDetectors;

public class X12EncodingDetector : IEdiEncodingDetector
{
    public InputType InputType => InputType.X12;

    public Encoding DetermineEncoding(byte[] fileBytes)
    {
        throw new NotImplementedException();
    }
}

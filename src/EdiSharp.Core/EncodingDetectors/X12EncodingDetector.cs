using EdiSharp.Core.Enums;
using EdiSharp.Core.Interfaces;
using System.Text;

namespace EdiSharp.Core.EncodingDetectors;

public class X12EncodingDetector : IEdiEncodingDetector
{
    public EdiStandard InputType => EdiStandard.X12;

    public Encoding DetermineEncoding(byte[] fileBytes)
    {
        if (fileBytes.StartsWith(new byte[] { 0xEF, 0xBB, 0xBF }))
            return Encoding.UTF8;

        if (fileBytes.StartsWith(new byte[] { 0xFF, 0xFE }))
            return Encoding.Unicode;

        if (fileBytes.StartsWith(new byte[] { 0xFE, 0xFF }))
            return Encoding.BigEndianUnicode;

        return Encoding.Latin1;
    }
}

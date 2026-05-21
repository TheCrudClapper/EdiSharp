using EdiSharp.Core.Enums;
using EdiSharp.Core.Interfaces;
using System.Text;

namespace EdiSharp.Core.EncodingDetectors;

public class EdifactEncodingDetector : IEdiEncodingDetector
{
    public InputType InputType => InputType.EDIFACT;

    public Encoding DetermineEncoding(byte[] fileBytes)
    {
        var head = Encoding.ASCII
           .GetString(fileBytes[..Math.Min(fileBytes.Length, 200)]);

        if (!head.Contains("UNB"))
            return Encoding.UTF8;

        var start = head.IndexOf("UNB");

        var segment = head[start..];

        if (segment.Contains("UNOC"))
            return Encoding.GetEncoding("ISO-8859-1");

        if (segment.Contains("UNOA"))
            return Encoding.ASCII;

        return Encoding.UTF8;
    }
}

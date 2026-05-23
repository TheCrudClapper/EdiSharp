using EdiSharp.Core.Enums;
using EdiSharp.Core.Interfaces;
using System.Text;

namespace EdiSharp.Core.EncodingDetectors;

public class EdifactEncodingDetector : IEdiEncodingDetector
{
    public InputType InputType => InputType.EDIFACT;

    public Encoding DetermineEncoding(byte[] fileBytes)
    {
        var head = Encoding.Latin1.GetString(
            fileBytes.AsSpan(0, Math.Min(fileBytes.Length, 200))
        ).AsSpan();

        var idx = head.IndexOf("UNB", StringComparison.Ordinal);
        if (idx < 0)
            return Encoding.UTF8;

        var segment = head[idx..];

        if (segment.Contains("UNOC", StringComparison.Ordinal))
            return Encoding.GetEncoding("ISO-8859-1");
        if (segment.Contains("UNOA", StringComparison.Ordinal))
            return Encoding.ASCII;

        return Encoding.UTF8;
    }
}

using EdiSharp.Core.Enums;
using EdiSharp.Core.Interfaces;
using System.Text;

namespace EdiSharp.Core.EncodingDetectors;

public class EdifactEncodingDetector : IEdiEncodingDetector
{
    public InputType InputType => InputType.EDIFACT;

    public Encoding DetermineEncoding(byte[] fileBytes)
    {
        ReadOnlySpan<byte> span = fileBytes;
        int unbIdx = span.IndexOf("UNB"u8);
        if (unbIdx == -1)
            return Encoding.UTF8;

        var slice = span.Slice(unbIdx, Math.Min(200, span.Length - unbIdx));

        if (slice.IndexOf("UNOC"u8) >= 0)
            return Encoding.GetEncoding("ISO-8859-1");

        if (slice.IndexOf("UNOA"u8) >= 0)
            return Encoding.ASCII;

        return Encoding.UTF8;
    }
}

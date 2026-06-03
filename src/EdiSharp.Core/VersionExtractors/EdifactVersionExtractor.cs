using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models.Delimiters;
using System.Text;

namespace EdiSharp.Core.VersionExtractors;

public class EdifactVersionExtractor : IEdiVersionExtractor
{
    public EdiStandard InputType => EdiStandard.EDIFACT;

    public string? Extract(byte[] fileBytes, Encoding encoding, EdiDelimiters delimiters)
    {
        var text = encoding.GetString(fileBytes);
        var startIndex = text.IndexOf("UNH+");
        if (startIndex == -1)
            return null;

        var endIndex = text.IndexOf(delimiters.SegmentTerminator, startIndex);
        if (endIndex == -1)
            return null;

        var segment = text.Substring(startIndex, endIndex - startIndex);

        var parts = segment.Split(delimiters.ElementSeparator);
        if (parts.Length < 3)
            return null;

        var messageIdParts = parts[2].Split(delimiters.ComponentSeparator);
        if (messageIdParts.Length < 3)
            return null;

        return $"{messageIdParts[0]} {messageIdParts[1]}.{messageIdParts[2]}";
    }

}

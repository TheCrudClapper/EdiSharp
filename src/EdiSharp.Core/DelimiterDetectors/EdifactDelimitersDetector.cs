using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.DelimiterDetectors;

public class EdifactDelimitersDetector : IEdiDelimiterDetector
{
    public InputType InputType => InputType.EDIFACT;

    public EdifactDelimiters DetectDelimiters(byte[] fileBytes, Encoding encoding)
    {
        var text = encoding.GetString(fileBytes);

        var firstLine = text.Length >= 9 ? text[..9] : text;

        if (firstLine.StartsWith("UNA"))
        {
            return new EdifactDelimiters()
            {
                ComponentSeparator = firstLine[3],
                ElementSeparator = firstLine[4],
                DecimalSeparator = firstLine[5],
                ReleaseCharacter = firstLine[6],
                RepetitionSeparator = firstLine[7],
                SegmentTerminator = firstLine[8]
            };
        }

        return new EdifactDelimiters();
    }
}

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
        var textSpan = encoding.GetString(fileBytes).AsSpan();

        var unaIndex = textSpan.IndexOf("UNA", StringComparison.Ordinal);

        if(unaIndex != -1) 
        {
            var isLineStart = 
                unaIndex == 0 
                || textSpan[unaIndex - 1] == '\n' 
                || textSpan[unaIndex - 1] == '\r';

            if (isLineStart && textSpan.Length >= unaIndex + 9)
            {
                var una = textSpan.Slice(unaIndex, 9);

                return new EdifactDelimiters
                {
                    ComponentSeparator = una[3],
                    ElementSeparator = una[4],
                    DecimalSeparator = una[5],
                    ReleaseCharacter = una[6],
                    RepetitionSeparator = una[7],
                    SegmentTerminator = una[8]
                };
            }
        }

        return EdifactDelimiters.DefaultEdifact();
    }
}

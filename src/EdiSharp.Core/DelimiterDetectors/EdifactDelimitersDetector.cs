using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.DelimiterDetectors;

public class EdifactDelimitersDetector : IEdiDelimiterDetector
{
    public InputType InputType => InputType.EDIFACT;

    public EdiDelimiters DetectDelimiters(byte[] fileBytes, Encoding encoding)
    {
        ReadOnlySpan<byte> textSpan = fileBytes.AsSpan();

        var unaIndex = textSpan.IndexOf("UNA"u8);

        if(unaIndex != -1) 
        {
            var isLineStart = 
                unaIndex == 0 
                || textSpan[unaIndex - 1] == '\n' 
                || textSpan[unaIndex - 1] == '\r';

            if (isLineStart && textSpan.Length >= unaIndex + 9)
            {
                var una = textSpan.Slice(unaIndex, 9);

                return new EdiDelimiters
                {
                    ComponentSeparator = (char)una[3],
                    ElementSeparator = (char)una[4],
                    DecimalSeparator = (char)una[5],
                    EscapeCharacter = (char)una[6],
                    RepetitionSeparator = (char)una[7],
                    SegmentTerminator = (char)una[8]
                };
            }
        }

        return EdiDelimiters.DefaultEdifact();
    }
}

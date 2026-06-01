using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Lexer;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.Tokenizers;

public class EdifactTokenizer : IEdiTokenizer
{
    public InputType InputType => InputType.EDIFACT;

    public List<EdiSegment> Tokenize(byte[] fileBytes, Encoding encoding, EdiDelimiters delimiters)
    {
        var text = encoding.GetString(fileBytes);
        var rawSegments = text.Split(delimiters.SegmentTerminator);
        var segments = new List<EdiSegment>();

        foreach (var raw in rawSegments)
        {
            var segment = raw.Trim();

            if (string.IsNullOrWhiteSpace(segment))
                continue;

            if (segment.StartsWith("UNA"))
                continue;

            var firstElemSeparatorIdx = segment.IndexOf(delimiters.ElementSeparator);

            string tag = string.Empty;
            List<string> elementsSplit = [];

            if (firstElemSeparatorIdx < 0)
            {
                tag = segment;
            }
            else
            {
                tag = segment[..firstElemSeparatorIdx];
                elementsSplit = EdiFileSplitter.Split(segment[(firstElemSeparatorIdx + 1)..],
                    delimiters.ElementSeparator,
                    delimiters.EscapeCharacter);
            }

            var elements = elementsSplit
                .Select(e => new EdiElement
                {
                    Raw = e,
                    Components = EdiFileSplitter.Split(e,
                        delimiters.ComponentSeparator,
                        delimiters.EscapeCharacter)
                })
                .ToList();

            segments.Add(new EdiSegment
            {
                Elements = elements,
                Tag = tag
            });
        }

        return segments;
    }

}

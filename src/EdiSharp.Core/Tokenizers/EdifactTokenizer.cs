using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
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

            string tag;
            string[] elementsRaw;

            if (firstElemSeparatorIdx < 0)
            {
                tag = segment;
                elementsRaw = [];
            }
            else
            {
                tag = segment[..firstElemSeparatorIdx];

                elementsRaw = segment[(firstElemSeparatorIdx + 1)..]
                    .Split(delimiters.ElementSeparator);
            }

            var elements = elementsRaw
                .Select(e => new EdiElement
                {
                    Raw = e,
                    Components = e.Split(delimiters.ComponentSeparator)
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

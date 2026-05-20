using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.Tokenizers;

public class EdifactTokenizer : IEdiTokenizer
{
    public InputType InputType => InputType.EDIFACT;

    public List<EdiSegment> Tokenize(byte[] fileBytes)
    {
        var delimiters = DetermineDelimiters(fileBytes);

        var text = Encoding.UTF8.GetString(fileBytes);
        var rawSegments = text.Split(delimiters.SegmentTerminator);

        var segments = new List<EdiSegment>();

        foreach (var raw in rawSegments)
        {
            var segment = raw.Trim();

            if (string.IsNullOrWhiteSpace(segment))
                continue;

            if (segment.StartsWith("UNA"))
                continue;

            var firstSeparatorIndex = segment.IndexOf(delimiters.ElementSeparator);

            string tag;
            string[] elementsRaw;

            if (firstSeparatorIndex < 0) 
            {
                tag = segment;
                elementsRaw = [];
            }
            else 
            {
                tag = segment[..firstSeparatorIndex];

                elementsRaw = segment[(firstSeparatorIndex + 1)..]
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

    private EdifactDelimiters DetermineDelimiters(byte[] fileBytes)
    {
        var text = Encoding.UTF8.GetString(fileBytes);

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

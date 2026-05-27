using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.DelimiterDetectors;

public class X12DelimiterDetector : IEdiDelimiterDetector
{
    public InputType InputType => InputType.X12;

    public EdiDelimiters DetectDelimiters(byte[] fileBytes, Encoding encoding)
    {
        var text = encoding.GetString(fileBytes);

        var isaStartIdx = text.IndexOf("ISA");
        
        if(isaStartIdx == -1)
            throw new Exception("Valid X12 file needs to have ISA segment.");

        var isa = text.AsSpan(isaStartIdx, 106);

        return new EdiDelimiters
        {
            ElementSeparator = isa[3],
            ComponentSeparator = isa[103],
            SegmentTerminator = isa[104],
            RepetitionSeparator = isa[82] == 'U' ? null : isa[82]
        };
    }
}

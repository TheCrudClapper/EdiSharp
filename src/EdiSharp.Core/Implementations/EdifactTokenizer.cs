using EdiSharp.Core.Enums;
using EdiSharp.Core.Interfaces;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.Implementations;

public class EdifactTokenizer : IEdiTokenizer
{
    public InputType InputType => InputType.EDIFACT;

    public List<EdiSegment> Tokenize(byte[] fileBytes)
    {
        var delimiters = DetermineDelimiters(fileBytes);
        throw new NotImplementedException();
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

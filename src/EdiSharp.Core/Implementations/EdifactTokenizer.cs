using EdiSharp.Core.Enums;
using EdiSharp.Core.Interfaces;
using EdiSharp.Core.Models;

namespace EdiSharp.Core.Implementations;

public class EdifactTokenizer : IEdiTokenizer
{
    public InputType InputType => InputType.EDIFACT;

    public List<EdiSegment> Tokenize(byte[] fileBytes)
    {
        throw new NotImplementedException();
    }

    private EdifactDelimiters DetermineDelimiters(byte[] fileBytes) 
    {
        throw new NotImplementedException();
    }
}

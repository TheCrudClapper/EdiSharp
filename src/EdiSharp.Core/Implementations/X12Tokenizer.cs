using EdiSharp.Core.Enums;
using EdiSharp.Core.Interfaces;
using EdiSharp.Core.Models;

namespace EdiSharp.Core.Implementations;

public class X12Tokenizer : IEdiTokenizer
{
    public InputType InputType => InputType.X12;

    public List<EdiSegment> Tokenize(byte[] fileBytes)
    {
        throw new NotImplementedException();
    }
}

using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;
using EdiSharp.Core.Models;

namespace EdiSharp.Core.Tokenizers;

public class X12Tokenizer : IEdiTokenizer
{
    public InputType InputType => InputType.X12;

    public List<EdiSegment> Tokenize(byte[] fileBytes)
    {
        throw new NotImplementedException();
    }
}

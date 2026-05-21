using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.Tokenizers;

public class X12Tokenizer : IEdiTokenizer
{
    public InputType InputType => InputType.X12;

    public List<EdiSegment> Tokenize(byte[] fileBytes, Encoding encoding, EdifactDelimiters delimiters)
    {
        throw new NotImplementedException();
    }
}

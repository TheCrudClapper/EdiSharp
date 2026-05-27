using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.Abstractions;

public interface IEdiTokenizer
{
    InputType InputType { get; }
    List<EdiSegment> Tokenize(byte[] fileBytes, Encoding encoding, EdiDelimiters delimiters);
}

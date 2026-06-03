using EdiSharp.Core.Enums;
using EdiSharp.Core.Models.Delimiters;
using EdiSharp.Core.Models.IntermediateModel;
using System.Text;

namespace EdiSharp.Core.Abstractions;

public interface IEdiTokenizer
{
    EdiStandard InputType { get; }
    List<EdiSegment> Tokenize(byte[] fileBytes, Encoding encoding, EdiDelimiters delimiters);
}

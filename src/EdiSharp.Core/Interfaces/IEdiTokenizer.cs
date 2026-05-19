using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;

namespace EdiSharp.Core.Interfaces;

public interface IEdiTokenizer
{
    InputType InputType { get; }
    List<EdiSegment> Tokenize(byte[] fileBytes);
}

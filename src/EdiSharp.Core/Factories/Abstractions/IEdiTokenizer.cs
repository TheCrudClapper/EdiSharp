using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;

namespace EdiSharp.Core.Factories.Abstractions;

public interface IEdiTokenizer
{
    InputType InputType { get; }
    List<EdiSegment> Tokenize(byte[] fileBytes);
}

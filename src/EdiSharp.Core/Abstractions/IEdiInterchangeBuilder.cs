using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.Abstractions;

public interface IEdiInterchangeBuilder
{
    InputType InputType { get; }
    EdiInterchange Build(List<EdiSegment> segments, Encoding encoding);
}

using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.Abstractions;

public interface IEdiMessageSplitter
{
    InputType InputType { get; }
    EdiInterchange Split(List<EdiSegment> segments, Encoding encoding);
}

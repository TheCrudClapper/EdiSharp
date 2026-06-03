using EdiSharp.Core.Enums;
using EdiSharp.Core.Models.IntermediateModel;
using System.Text;

namespace EdiSharp.Core.Abstractions;

public interface IEdiInterchangeBuilder
{
    EdiStandard InputType { get; }
    EdiInterchange Build(List<EdiSegment> segments);
}

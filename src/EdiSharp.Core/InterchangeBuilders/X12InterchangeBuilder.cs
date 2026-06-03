using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models.IntermediateModel;
using System.Text;

namespace EdiSharp.Core.MessageSplitters;

public class X12InterchangeBuilder : IEdiInterchangeBuilder
{
    public EdiStandard InputType => EdiStandard.X12;

    public EdiInterchange Build(List<EdiSegment> segments)
    {
        throw new NotImplementedException();
    }
}

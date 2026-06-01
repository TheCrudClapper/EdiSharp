using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.MessageSplitters;

public class X12InterchangeBuilder : IEdiInterchangeBuilder
{
    public InputType InputType => InputType.X12;

    public EdiInterchange Build(List<EdiSegment> segments, Encoding encoding)
    {
        throw new NotImplementedException();
    }
}

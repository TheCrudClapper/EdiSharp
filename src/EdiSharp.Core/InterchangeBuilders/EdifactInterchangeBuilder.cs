using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.MessageSplitters;

public class EdifactInterchangeBuilder : IEdiInterchangeBuilder
{
    public InputType InputType => InputType.EDIFACT;

    public EdiInterchange Build(List<EdiSegment> segments, Encoding encoding)
    {
        throw new NotImplementedException();
    }
}

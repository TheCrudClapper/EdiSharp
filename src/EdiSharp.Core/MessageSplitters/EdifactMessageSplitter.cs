using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.MessageSplitters;

public class EdifactMessageSplitter : IEdiMessageSplitter
{
    public InputType InputType => InputType.EDIFACT;

    public EdiInterchange Split(List<EdiSegment> segments, Encoding encoding)
    {
        throw new NotImplementedException();
    }
}

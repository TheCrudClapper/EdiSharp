using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.MessageSplitters;

public class X12MessageSplitter : IEdiMessageSplitter
{
    public InputType InputType => InputType.X12;

    public EdiInterchange Split(List<EdiSegment> segments, Encoding encoding)
    {
        throw new NotImplementedException();
    }
}

using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models.IntermediateModel;
using System.Globalization;

namespace EdiSharp.Core.MessageSplitters;

public class EdifactInterchangeBuilder : IEdiInterchangeBuilder
{
    public EdiStandard InputType => EdiStandard.EDIFACT;

    public EdiInterchange Build(List<EdiSegment> segments)
    {
        EdiInterchange? interchange = null;
        List<EdiMessage> messages = [];
        List<EdiSegment> currentSegments = [];
        string? messageRefNumber = null;
        MessageIdentifier? messageIdentifier = null;
        InterchangeHeader? interchangeHeader = null;
        InterchangeTrailer? interchangeTrailer = null;
        EdiMessage? currentMessage = null;

        foreach(var seg in segments)
        {
            if (seg.Tag == "UNB")
            {
                var date = seg.Elements[3].Components[0];
                var time = seg.Elements[3].Components[1];
                interchangeHeader = new InterchangeHeader()
                {
                    SenderId = seg.Elements[1].Components[0],
                    RecieverId = seg.Elements[2].Components[0],
                    ControlReference = seg.Elements[4].Components[0],
                    DateTime = DateTime.ParseExact(date + time, "yyMMddHHmm", CultureInfo.InvariantCulture)
                };

                continue;
            }

            if(seg.Tag == "UNH") 
            {
                messageRefNumber = seg.Elements[0].Components[0];
                messageIdentifier = new MessageIdentifier()
                {
                    Type = seg.Elements[1].Components[0],
                    Version = seg.Elements[1].Components[1],
                    Release = seg.Elements[1].Components[2]
                };

                continue;
            }

            if(seg.Tag == "UNT")
            {
                currentMessage = new EdiMessage()
                {
                    Identifier = messageIdentifier,
                    ReferenceNumber = messageRefNumber,
                    Segments = currentSegments.ToList(),
                };



                messages.Add(currentMessage);

                currentSegments.Clear();
                currentMessage = null;
                messageIdentifier = null;
                messageRefNumber = null;

                continue;
            }

            if(seg.Tag == "UNZ") 
            {
                interchangeTrailer = new InterchangeTrailer()
                {
                    MessageCount = int.Parse(seg.Elements[0].Components[0]),
                    ControlReference = seg.Elements[1].Components[0],
                };

                interchange = new EdiInterchange()
                {
                    Header = interchangeHeader,
                    Messages = messages,
                    Trailer = interchangeTrailer,
                    Standard = EdiStandard.EDIFACT,
                };

                continue;
            }

            currentSegments.Add(seg);
        }

        return interchange;
    }
}

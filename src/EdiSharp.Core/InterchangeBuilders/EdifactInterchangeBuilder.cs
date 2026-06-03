using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Exceptions;
using EdiSharp.Core.Models.IntermediateModel;
using System.Globalization;

namespace EdiSharp.Core.MessageSplitters;

public class EdifactInterchangeBuilder : IEdiInterchangeBuilder
{
    public EdiStandard InputType => EdiStandard.EDIFACT;
    private sealed class BuildContext
    {
        public InterchangeHeader? InterchangeHeader { get; set; }
        public InterchangeTrailer? InterchangeTrailer { get; set; }
        public List<EdiMessage> Messages { get; } = [];
        public List<EdiSegment> CurrentMessageSegments { get; set; } = [];
        public MessageHeader? MessageHeader { get; set; }
        public MessageTrailer? MessageTrailer { get; set; }
    }

    public EdiInterchange Build(List<EdiSegment> segments)
    {
        var ctx = new BuildContext();

        foreach (var seg in segments)
        {
            switch (seg.Tag)
            {
                case "UNB":
                    HandleUNB(seg, ctx);
                    break;
                case "UNH":
                    HandleUNH(seg, ctx);
                    break;
                case "UNT":
                    HandleUNT(seg, ctx);
                    break;
                case "UNZ":
                    HandleUNZ(seg, ctx);
                    break;
                default:
                    HandlePayload(seg, ctx);
                    break;
            }
        }

        return new EdiInterchange
        {
            Header = ctx.InterchangeHeader ?? throw new EdiSemanticsException("Missing UNB"),
            Trailer = ctx.InterchangeTrailer ?? throw new EdiSemanticsException("Missing UNZ"),
            Messages = ctx.Messages,
            Standard = EdiStandard.EDIFACT
        };
    }

    private static void HandleUNB(EdiSegment seg, BuildContext ctx)
    {
        var date = seg.Elements[3].Components[0];
        var time = seg.Elements[3].Components[1];

        ctx.InterchangeHeader = new InterchangeHeader
        {
            SenderId = seg.Elements[1].Components[0],
            RecieverId = seg.Elements[2].Components[0],
            ControlReference = seg.Elements[4].Components[0],
            DateTime = DateTime.ParseExact(date + time, "yyMMddHHmm", CultureInfo.InvariantCulture)
        };
    }

    private static void HandleUNH(EdiSegment seg, BuildContext ctx)
    {
        var identifier = seg.Elements[1];

        ctx.MessageHeader = new MessageHeader()
        {
            MessageReferenceNumber = seg.Elements[0].Components[0],
            Type = identifier.Components[0],
            Version = identifier.Components[1],
            Release = identifier.Components[2]
        };

        ctx.CurrentMessageSegments = [];
    }

    private static void HandleUNT(EdiSegment seg, BuildContext ctx)
    {
        if (ctx.MessageHeader is null)
            throw new EdiSemanticsException("UNT without UNH");

        ctx.MessageTrailer = new MessageTrailer()
        {
            //Add one, to count UNH as well
            SegmentCount = int.Parse(seg.Elements[0].Components[0]) + 1,
            MessageReferenceNumber = seg.Elements[1].Components[0]
        };

        var message = new EdiMessage()
        {
            Trailer = ctx.MessageTrailer,
            Header = ctx.MessageHeader,
            Segments = ctx.CurrentMessageSegments.ToList()
        };

        ctx.Messages.Add(message);

        ctx.MessageTrailer = null;
        ctx.MessageHeader = null;
        ctx.CurrentMessageSegments = [];
    }

    private static void HandleUNZ(EdiSegment seg, BuildContext ctx)
    {
        ctx.InterchangeTrailer = new InterchangeTrailer()
        {
            MessageCount = int.Parse(seg.Elements[0].Components[0]),
            ControlReference = seg.Elements[1].Components[0],
        };
    }

    private static void HandlePayload(EdiSegment seg, BuildContext ctx) 
    {
        //Only when we are inside of message
        if(ctx.MessageHeader is not null) 
        {
            ctx.CurrentMessageSegments.Add(seg);
        }
    }
}
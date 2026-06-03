using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Exceptions;
using EdiSharp.Core.Models.IntermediateModel;
using System.Globalization;

namespace EdiSharp.Core.MessageSplitters;

// TO DO:
// Add States for Builder: WaitingForUNB, InsideMessage, InsideInterchange, Completed
// Add Support for Functional Groups, then support for message reference num validation if many messages
// Unit Testing 
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

        //When interchange concluded before message was saved by UNT
        if (ctx.MessageHeader is not null)
            throw new EdiSemanticsException("Message was not closed by UNT");

        return new EdiInterchange
        {
            Header = ctx.InterchangeHeader ?? throw new EdiSemanticsException("Missing UNB."),
            Trailer = ctx.InterchangeTrailer ?? throw new EdiSemanticsException("Missing UNZ."),
            Messages = ctx.Messages,
            Standard = EdiStandard.EDIFACT
        };
    }

    private static void HandleUNB(EdiSegment seg, BuildContext ctx)
    {
        if (ctx.InterchangeHeader is not null)
            throw new EdiSemanticsException("Interchange already opened.");

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
        if (ctx.InterchangeHeader is null)
            throw new EdiSemanticsException("Cannot build message without interchange.");

        if (ctx.MessageHeader is not null)
            throw new EdiSemanticsException("Message already opened.");

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
            throw new EdiSemanticsException("UNT without UNH.");

        if (ctx.MessageTrailer is not null)
            throw new EdiSemanticsException("UNT already defined.");

        ctx.MessageTrailer = new MessageTrailer()
        {
            SegmentCount = int.Parse(seg.Elements[0].Components[0]),
            MessageReferenceNumber = seg.Elements[1].Components[0]
        };

        if (ctx.MessageHeader.MessageReferenceNumber != ctx.MessageTrailer.MessageReferenceNumber)
            throw new EdiSemanticsException("Message Reference Number in Header and Trailer are different.");

        if(ctx.MessageTrailer.SegmentCount != ctx.CurrentMessageSegments.Count + 2)
            throw new EdiSemanticsException($"Segment count in message trailer: {ctx.MessageTrailer.SegmentCount} " +
                $"differs from actual: {ctx.CurrentMessageSegments.Count}");

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
        if (ctx.InterchangeHeader is null)
            throw new EdiSemanticsException("UNZ without UNB");

        if (ctx.InterchangeTrailer is not null)
            throw new EdiSemanticsException("UNZ already defined.");

        if (ctx.MessageHeader is not null && ctx.MessageTrailer is null)
            throw new EdiSemanticsException("Cannot close interchange while message is open.");

        ctx.InterchangeTrailer = new InterchangeTrailer()
        {
            MessageCount = int.Parse(seg.Elements[0].Components[0]),
            ControlReference = seg.Elements[1].Components[0],
        };

        if (ctx.InterchangeTrailer.MessageCount != ctx.Messages.Count)
            throw new EdiSemanticsException($"Message count in interchange trailer: {ctx.InterchangeTrailer.MessageCount} differs" +
                $" than actual: {ctx.Messages.Count}.");

        if (ctx.InterchangeHeader.ControlReference != ctx.InterchangeTrailer.ControlReference)
            throw new EdiSemanticsException($"Interchange control reference in header: {ctx.InterchangeHeader.ControlReference}" +
                $" and trailer: {ctx.InterchangeTrailer.ControlReference} are different.");
    }

    private static void HandlePayload(EdiSegment seg, BuildContext ctx)
    {
        //Only when we are inside of message
        if (ctx.MessageHeader is not null)
            ctx.CurrentMessageSegments.Add(seg);
        else
            throw new EdiSemanticsException("Payload segment outside message");

    }
}
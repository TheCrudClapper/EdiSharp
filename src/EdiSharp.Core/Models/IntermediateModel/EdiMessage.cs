namespace EdiSharp.Core.Models.IntermediateModel;

public record EdiMessage
{
    public required MessageHeader Header { get; init; }
    public IReadOnlyList<EdiSegment> Segments { get; init; } = [];
    public required MessageTrailer Trailer { get; init; }
}

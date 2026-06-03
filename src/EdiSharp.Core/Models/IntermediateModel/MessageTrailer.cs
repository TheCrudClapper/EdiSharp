namespace EdiSharp.Core.Models.IntermediateModel;

public record MessageTrailer
{
    public required int SegmentCount { get; init; }
    public required string? MessageReferenceNumber { get; init; }
}

namespace EdiSharp.Core.Models.IntermediateModel;

public record EdiMessage
{
    public required MessageIdentifier Identifier { get; init; }
    public string? ReferenceNumber { get; init; }
    public IReadOnlyList<EdiSegment> Segments { get; init; } = [];
}

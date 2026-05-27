namespace EdiSharp.Core.Models;

public class EdiMessage
{
    public required string Type { get; init; } = null!;
    public required IReadOnlyList<EdiSegment> Segments { get; init; }
}

namespace EdiSharp.Core.Models;

/// <summary>
/// Represents an EDI segment identified by its tag and containing an ordered collection of elements.
/// </summary>
/// <remarks>Defined as a record with required init-only properties. Tag is the segment identifier (for example,
/// "N1"). Elements contains the segment's EdiElement items in sequence; the list instance may be modified after
/// construction.</remarks>
public record EdiSegment
{
    public required string Tag { get; init; }
    public required IReadOnlyList<EdiElement> Elements { get; init; }
}

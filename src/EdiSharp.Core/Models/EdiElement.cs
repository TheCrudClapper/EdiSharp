namespace EdiSharp.Core.Models;

/// <summary>
/// Represents an EDI data element containing the original raw text and its parsed component values.
/// </summary>
/// <remarks>Both properties are required and init-only. Raw preserves the original element text as received.
/// Components contains the element's component values in source order and may be empty if the element has no
/// components.</remarks>
public record EdiElement
{
    public required string Raw { get; init; }
    public required IReadOnlyList<string> Components { get; init; }
}

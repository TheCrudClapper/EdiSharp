namespace EdiSharp.Core.Models.IntermediateModel;

public record MessageHeader
{
    public string? MessageReferenceNumber { get; init; }
    public required string Type { get; init; }
    public string? Version { get; init; }
    public string? Release { get; init; }
}

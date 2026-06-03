namespace EdiSharp.Core.Models.IntermediateModel;
public record MessageIdentifier
{
    public required string Type { get; init; }
    public string? Version { get; init; }
    public string? Release { get; init;  }
}

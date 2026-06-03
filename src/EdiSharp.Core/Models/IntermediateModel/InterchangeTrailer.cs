namespace EdiSharp.Core.Models.IntermediateModel;

public record InterchangeTrailer
{
    public required int MessageCount { get; init; }
    public required string ControlReference { get; init; }
}

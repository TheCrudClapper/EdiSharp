namespace EdiSharp.Core.Models.IntermediateModel;

public record InterchangeHeader
{
    public required string? SenderId { get; init; }
    public required string? RecieverId { get; init; }
    public required string? ControlNumer { get; init; }
    public DateTime? DateTime { get; init; }
}

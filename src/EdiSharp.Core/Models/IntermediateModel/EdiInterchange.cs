using EdiSharp.Core.Enums;

namespace EdiSharp.Core.Models.IntermediateModel;

public record EdiInterchange
{  
    public EdiStandard Standard { get; init; }
    public required InterchangeHeader Header { get; init; }
    public required IReadOnlyList<EdiMessage> Messages { get; init; }
    public required InterchangeTrailer Trailer { get; init; }
}

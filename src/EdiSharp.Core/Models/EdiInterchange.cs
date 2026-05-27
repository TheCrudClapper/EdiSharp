namespace EdiSharp.Core.Models;

public class EdiInterchange
{
    public required IReadOnlyList<EdiMessage> Messages { get; init; }
}

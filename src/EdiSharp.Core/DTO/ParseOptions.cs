using EdiSharp.Core.Enums;
using System.Net.Http.Headers;

namespace EdiSharp.Core.DTO;
public record ParseOptions
{
    public required InputType InputType { get; init; }
    public required OutputType OutputType { get; init; }
    public required bool Validate { get; init; }
    public required bool ShowRawSegments { get; init; }
}

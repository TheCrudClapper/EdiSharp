using EdiSharp.Core.Enums;
using EdiSharp.Core.Models.Delimiters;
using System.Text;

namespace EdiSharp.Core.DTO;

public record ParseOptions
{
    public required EdiStandard InputType { get; init; }
    public required Encoding Encoding { get; init; }
    public required EdiDelimiters Delimiters { get; init; }
    public required OutputType OutputType { get; init; }
    public required bool Validate { get; init; }
}

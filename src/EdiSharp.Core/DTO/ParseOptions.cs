using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.DTO;

public record ParseOptions
{
    public required InputType InputType { get; init; }
    public required Encoding Encoding { get; init; }
    public required EdiDelimiters Delimiters { get; init; }
    public required OutputType OutputType { get; init; }
    public required bool Validate { get; init; }
}

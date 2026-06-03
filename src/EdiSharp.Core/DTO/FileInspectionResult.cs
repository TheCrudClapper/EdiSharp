using EdiSharp.Core.Enums;
using EdiSharp.Core.Models.Delimiters;
using System.Text;

namespace EdiSharp.Core.DTO;

public record FileInspectionResult
{
    public int SegmentCount { get; init; }
    public Encoding Encoding { get; init; } = null!;
    public string Version { get; init; } = null!;
    public EdiStandard InputType { get; init; }
    public EdiDelimiters Delimiters { get; init; } = null!;
}

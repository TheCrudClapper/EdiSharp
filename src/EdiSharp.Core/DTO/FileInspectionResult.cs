using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.DTO;

public record FileInspectionResult
{
    public int SegmentCount { get; init; }
    public Encoding Encoding { get; init; } = null!;
    public string Version { get; init; } = null!;
    public InputType InputType { get; init; }
    public EdifactDelimiters Delimiters { get; init; } = null!;
}

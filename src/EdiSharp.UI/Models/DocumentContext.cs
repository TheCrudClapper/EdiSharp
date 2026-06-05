using EdiSharp.Core.DTO;

namespace EdiSharp.UI.Models;

public sealed class DocumentContext
{
    public required byte[] Bytes { get; init; }
    public required FileInspectionResult Inspection { get; init; }
    public required string FileName { get; init; }
    public string EncodingName { get; init; } = null!;
    public string EdiStandard { get; init; } = null!;
    public string InputEdiStandard { get; init; } = null!;
    public string? RawPreview { get; set; }
}

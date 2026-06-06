using EdiSharp.Core.DTO;

namespace EdiSharp.UI.Models;

public sealed class DocumentContext
{
    public required byte[] Bytes { get; init; }
    public required FileInspectionResult Inspection { get; init; }
}

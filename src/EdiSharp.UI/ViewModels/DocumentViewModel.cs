namespace EdiSharp.UI.ViewModels;

public class DocumentViewModel
{
    private const string DefaultText = "Unknown";
    public required string FileName { get; set; } = null!;
    public int SegmentCount { get; set; } = 0;
    public string EncodingName { get; set; } = null!;
    public string EdiStandard { get; set; } = null!;
    public string? RawPreview { get; set; }

    public static DocumentViewModel GetInitalState()
    {
        return new DocumentViewModel()
        {
            FileName = "File not selected",
            EncodingName = DefaultText,
            EdiStandard = DefaultText,
            RawPreview = "Select file to see preview",
            SegmentCount = 0,
        };
    }
}

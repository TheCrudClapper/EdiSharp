using EdiSharp.Core.DTO;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;
using EdiSharp.Core.Models;
using EdiSharp.Core.ServiceContracts;
using EdiSharp.Domain.ResultTypes;
using System.Text;

namespace EdiSharp.Core.Services;

public class FileInspectionService : IFileInspectionService
{
    private readonly IEdiEncodingDetectorFactory _detectorFactory;
    private readonly IEdiDelimiterDetectorFactory _delimiterFactory;
    public FileInspectionService(IEdiEncodingDetectorFactory detectorFactory, IEdiDelimiterDetectorFactory delimiterFactory)
    {
        _detectorFactory = detectorFactory;
        _delimiterFactory = delimiterFactory;
    }

    public Result<FileInspectionResult> Inspect(byte[] fileBytes)
    {
        var inputType = DetermineInputType(fileBytes);

        if (inputType is null)
            return Result.Failure<FileInspectionResult>(Error.Create("Provided file is not valid EDIFACT or X12 file"));

        var encodingDetector = _detectorFactory.Create(inputType.Value);
        Encoding encoding = encodingDetector.DetermineEncoding(fileBytes);

        var delimiterDetector = _delimiterFactory.Create(inputType.Value);
        var delimiters = delimiterDetector.DetectDelimiters(fileBytes, encoding);

        var rawPreview = BuildRawFilePreview(fileBytes, encoding);

        return new FileInspectionResult
        {
            Encoding = encoding,
            InputType = inputType.Value,
            SegmentCount = CountSegments(fileBytes, encoding, delimiters),
            RawDocument = rawPreview,
            Delimiters = delimiters,
            Version = "D96"
        };
    }

    private static InputType? DetermineInputType(byte[] fileBytes)
    {
        var head = Encoding.ASCII.GetString(
            fileBytes[..Math.Min(fileBytes.Length, 300)]);

        if (head.StartsWith("ISA"))
            return InputType.X12;

        if (head.StartsWith("UNA") ||
            head.StartsWith("UNB"))
            return InputType.EDIFACT;

        return null;
    }

    private static string BuildRawFilePreview(byte[] fileBytes, Encoding encoding)
    {
        var sb = new StringBuilder();
        var text = encoding.GetString(fileBytes);

        var lines = text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            sb.AppendLine($"{i + 1:000}: {lines[i].TrimEnd('\r')}");
        }

        return sb.ToString();
    }

    private static int CountSegments(byte[] fileBytes, Encoding encoding, EdifactDelimiters delimiters)
    {
        var text = encoding.GetString(fileBytes);
        return text.Split(delimiters.SegmentTerminator).Length;
    }
}

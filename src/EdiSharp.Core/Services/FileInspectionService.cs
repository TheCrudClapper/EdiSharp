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
    private readonly IEdiVersionExtractorFactory _extractorFactory;
    public FileInspectionService(IEdiEncodingDetectorFactory detectorFactory,
        IEdiDelimiterDetectorFactory delimiterFactory,
        IEdiVersionExtractorFactory extractorFactory)
    {
        _detectorFactory = detectorFactory;
        _delimiterFactory = delimiterFactory;
        _extractorFactory = extractorFactory;
    }

    public Result<FileInspectionResult> Inspect(byte[] fileBytes)
    {
        var inputType = DetermineInputType(fileBytes);

        if (inputType is null)
            return Result.Failure<FileInspectionResult>(Error.Create("File is not recognized as EDIFACT or X12. Expected ISA or UNB/UNA header."));

        var encodingDetector = _detectorFactory.TryCreate(inputType.Value);
        if (encodingDetector is null)
            return Result.Failure<FileInspectionResult>(Error.Create("Failed to detect file encoding. The file may contain unsupported or corrupted byte sequences."));

        Encoding encoding = encodingDetector.DetermineEncoding(fileBytes);

        var delimiterDetector = _delimiterFactory.TryCreate(inputType.Value);
        if (delimiterDetector is null)
            return Result.Failure<FileInspectionResult>(Error.Create("EDI delimiters could not be determined. Check if file uses standard EDIFACT or X12 syntax."));

        var delimiters = delimiterDetector.DetectDelimiters(fileBytes, encoding);

        var versionExtractor = _extractorFactory.TryCreate(inputType.Value);
        if (versionExtractor is null)
            return Result.Failure<FileInspectionResult>(Error.Create("Unable to extract message version from UNH segment. File may be malformed or non-EDIFACT."));

        var version = versionExtractor.Extract(fileBytes, encoding, delimiters);
        if (version is null)
            return Result.Failure<FileInspectionResult>(Error.Create("Unable to detect EDI file version. Ensure file is not corrupted or uses supported format."));

        return new FileInspectionResult
        {
            Encoding = encoding,
            InputType = inputType.Value,
            SegmentCount = CountSegments(fileBytes, encoding, delimiters),
            Delimiters = delimiters,
            Version = version,
        };
    }

    private static InputType? DetermineInputType(byte[] fileBytes)
    {
        var head = Encoding.Latin1.GetString(
            fileBytes[..Math.Min(fileBytes.Length, 300)])
            .TrimStart('\uFEFF', ' ', '\r', '\n', '\t');

        if (head.StartsWith("ISA"))
            return InputType.X12;

        if (head.StartsWith("UNA") ||
            head.StartsWith("UNB"))
            return InputType.EDIFACT;

        return null;
    }

    private static int CountSegments(byte[] fileBytes, Encoding encoding, EdifactDelimiters delimiters)
    {
        var sep = (byte)delimiters.SegmentTerminator;
        return fileBytes.Count(b => b == sep);
    }
}

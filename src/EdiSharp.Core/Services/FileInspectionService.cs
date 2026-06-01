using EdiSharp.Core.DTO;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Exceptions;
using EdiSharp.Core.Factories.Abstractions;
using EdiSharp.Core.Models;
using EdiSharp.Core.ServiceContracts;
using EdiSharp.Domain.Errors;
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
            return Result.Failure<FileInspectionResult>(FileInspectionErrors.FileNotRecognized);

        var encodingDetector = _detectorFactory.TryCreate(inputType.Value) 
            ?? throw new EdiInstantiationException($"No encoding detector for {inputType.Value} type");

        Encoding encoding = encodingDetector.DetermineEncoding(fileBytes);

        var delimiterDetector = _delimiterFactory.TryCreate(inputType.Value) 
            ?? throw new EdiInstantiationException($"No delimiter detector for {inputType.Value} type");

        var delimiters = delimiterDetector.DetectDelimiters(fileBytes, encoding);

        var versionExtractor = _extractorFactory.TryCreate(inputType.Value)
            ?? throw new EdiInstantiationException($"No version extractor for {inputType.Value} type");

        var version = versionExtractor.Extract(fileBytes, encoding, delimiters);
        if (version is null)
            return Result.Failure<FileInspectionResult>(FileInspectionErrors.UnableToDetectEdiVersion);

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
        ReadOnlySpan<byte> data = fileBytes;
        if (data.Length >= 3 &&
            data[0] == 0xEF &&
            data[1] == 0xBB &&
            data[2] == 0xBF)
        {
            data = data.Slice(3);
        }

        int i  = 0;

        while(i < data.Length) 
        {
            byte b = data[i];

            if (b != ' ' && b != '\r' && b != '\n' && b != '\t')
                break;

            i++;
        }

        data = data.Slice(i);

        if (data.Length >= 3 && data[0] == 'I' && data[1] == 'S' && data[2] == 'A')
            return InputType.X12;

        if (data.Length >= 3 && (data[0] == 'U' && data[1] == 'N' && data[2] == 'A') ||
            (data[0] == 'U' && data[1] == 'N' && data[2] == 'B'))
            return InputType.EDIFACT;

        return null;
    }

    private static int CountSegments(byte[] fileBytes, Encoding encoding, EdiDelimiters delimiters)
    {
        var sep = (byte)delimiters.SegmentTerminator;
        return fileBytes.Count(b => b == sep);
    }
}

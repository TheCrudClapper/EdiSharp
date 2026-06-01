using EdiSharp.Domain.ResultTypes;

namespace EdiSharp.Domain.Errors;

public static class FileInspectionErrors
{
    public static readonly Error FileNotRecognized =
        new("File is not recognized as EDIFACT or X12. Expected ISA or UNB/UNA header.");

    public static readonly Error UnableToDetectEdiVersion =
        new("Unable to detect EDI file version. Ensure file is not corrupted or uses supported format.");
}

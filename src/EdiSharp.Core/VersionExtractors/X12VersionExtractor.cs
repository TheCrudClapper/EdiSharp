using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models.Delimiters;
using System.Text;

namespace EdiSharp.Core.VersionExtractors;

public class X12VersionExtractor : IEdiVersionExtractor
{
    public EdiStandard InputType => EdiStandard.X12;

    public string? Extract(byte[] fileBytes, Encoding encoding, EdiDelimiters delimiters)
    {
        return "X12";
    }
}

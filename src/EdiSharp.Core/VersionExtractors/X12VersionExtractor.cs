using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.VersionExtractors;

public class X12VersionExtractor : IEdiVersionExtractor
{
    public InputType InputType => InputType.X12;

    public string? Extract(byte[] fileBytes, Encoding encoding, EdiDelimiters delimiters)
    {
        return "X12";
    }
}

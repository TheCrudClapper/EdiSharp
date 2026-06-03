using EdiSharp.Core.Enums;
using EdiSharp.Core.Models.Delimiters;
using System.Text;

namespace EdiSharp.Core.Abstractions;

public interface IEdiVersionExtractor
{
    EdiStandard InputType { get; }
    string? Extract(byte[] fileBytes, Encoding encoding, EdiDelimiters delimiters);
}
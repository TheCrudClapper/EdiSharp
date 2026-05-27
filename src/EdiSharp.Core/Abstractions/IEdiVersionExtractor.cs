using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.Abstractions;

public interface IEdiVersionExtractor
{
    InputType InputType { get; }
    string? Extract(byte[] fileBytes, Encoding encoding, EdiDelimiters delimiters);
}
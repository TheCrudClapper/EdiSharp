using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.Abstractions;

public interface IEdiDelimiterDetector
{
    InputType InputType { get; }
    EdiDelimiters DetectDelimiters(byte[] fileBytes, Encoding encoding);
}

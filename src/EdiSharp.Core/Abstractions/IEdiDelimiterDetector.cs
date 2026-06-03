using EdiSharp.Core.Enums;
using EdiSharp.Core.Models.Delimiters;
using System.Text;

namespace EdiSharp.Core.Abstractions;

public interface IEdiDelimiterDetector
{
    EdiStandard InputType { get; }
    EdiDelimiters DetectDelimiters(byte[] fileBytes, Encoding encoding);
}

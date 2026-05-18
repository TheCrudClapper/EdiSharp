using EdiSharp.Core.Enums;
using System.Net.Http.Headers;

namespace EdiSharp.Core.DTO;
public class ParseOptions
{
    InputType InputType { get; set; }
    OutputType OutputType { get; set; }
    bool Validate { get; set; }
    bool ShowRawSegments { get; set; }

}

using EdiSharp.Core.Enums;

namespace EdiSharp.UI.Helpers;

public static class InputTypeToStringConverter
{
    public static string ToStringInputType(EdiStandard inputType)
    {
        return inputType switch
        {
            EdiStandard.EDIFACT => "EDIFACT",
            EdiStandard.X12 => "X12",
            _ => "Unknown"
        };
    }
}

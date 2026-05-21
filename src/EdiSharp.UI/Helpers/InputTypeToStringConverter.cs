using EdiSharp.Core.Enums;

namespace EdiSharp.UI.Helpers;

public static class InputTypeToStringConverter
{
    public static string ToStringInputType(InputType inputType)
    {
        return inputType switch
        {
            InputType.EDIFACT => "EDIFACT",
            InputType.X12 => "X12",
            _ => "Unknown"
        };
    }
}

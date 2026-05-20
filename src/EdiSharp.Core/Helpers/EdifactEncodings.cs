using System.Text;

namespace EdiSharp.Core.Helpers;

/// <summary>
/// Helper class that matches segment tag with appropiate encoding used in edi file.
/// </summary>
public static class EdifactEncodings
{
    public static Encoding FromSyntaxIdentifier(string syntax)
    {
        return syntax switch
        {
            "UNOA" => Encoding.ASCII,
            "UNOB" => Encoding.GetEncoding("ISO-8859-1"),
            "UNOC" => Encoding.GetEncoding("ISO-8859-1"),
            "UNOD" => Encoding.UTF8,
            _ => Encoding.UTF8,
        };
    }
}

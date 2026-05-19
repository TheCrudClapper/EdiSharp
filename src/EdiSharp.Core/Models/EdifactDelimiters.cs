namespace EdiSharp.Core.Models;

/// <summary>
/// Represents edifact delimiters, with default variants
/// </summary>
public class EdifactDelimiters
{

    //Separates components 
    public char ComponentSeparator { get; set; } = ':';

    //Separates data elements in segment
    public char ElementSeparator { get; init; } = '+';

    //Ends whole segment
    public char SegmentTerminator { get; init; } = '\'' ;

    //Makes next delimiter being treated as normal character not delimiters
    public char ReleaseCharacter { get; init; } = '?';

    public char RepetitionSeparator { get; init; } = '*';

    //Specifies which separator is used for decimal places
    public char DecimalSeparator { get; init; } = '.';
}

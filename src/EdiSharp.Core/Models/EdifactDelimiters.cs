namespace EdiSharp.Core.Models;

/// <summary>
/// Defines the set of EDIFACT delimiter characters used to parse and generate segments, data elements, components,
/// repetitions, release escapes, and decimal separators.
/// </summary>
/// <remarks>Defaults: ComponentSeparator=':', ElementSeparator='+', SegmentTerminator (single quote, '),
/// ReleaseCharacter='?', RepetitionSeparator='*', DecimalSeparator='.'. Intended to be provided to parsers and writers
/// or adjusted to match UNA/UNB header values or local conventions.</remarks>
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

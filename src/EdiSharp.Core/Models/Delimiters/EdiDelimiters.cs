namespace EdiSharp.Core.Models.Delimiters;

/// <summary>
/// Represents the delimiter characters used for parsing and writing EDI messages (EDIFACT and X12).
/// </summary>
/// <remarks>Provides properties for component, element, segment, release, repetition and decimal separators.
/// Includes DefaultEdifact and DefaultX12 factory methods for common predefined sets. ComponentSeparator is mutable;
/// the other separators are init-only.</remarks>
public class EdiDelimiters
{

    //Separates components 
    public char ComponentSeparator { get; set; }

    //Separates data elements in segment
    public char ElementSeparator { get; init; }

    //Ends whole segment
    public char SegmentTerminator { get; init; }

    //Makes next delimiter being treated as normal character not delimiters
    public char EscapeCharacter { get; init; }

    public char? RepetitionSeparator { get; init; }

    //Specifies which separator is used for decimal places
    public char DecimalSeparator { get; init; }

    public static EdiDelimiters DefaultEdifact() 
    {
        return new EdiDelimiters
        {
            ComponentSeparator = ':',
            ElementSeparator = '+',
            DecimalSeparator = '.',
            EscapeCharacter = '?',
            RepetitionSeparator = '*',
            SegmentTerminator = '\''
        };
    }
}

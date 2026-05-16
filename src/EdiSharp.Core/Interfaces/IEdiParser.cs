using EdiSharp.Core.Models;

namespace EdiSharp.Core.Interfaces;

//Parse Segments to Intermediate EdiDocument Model
public interface IEdiParser
{
    EdiDocument Parse(string input);
}

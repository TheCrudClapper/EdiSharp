using EdiSharp.Core.Models;
using EdiSharp.Core.Models.IntermediateModel;

namespace EdiSharp.Core.Abstractions;

//Parse Segments to Intermediate EdiDocument Model
public interface IEdiParser
{
    EdiInterchange Parse(string input);
}

using EdiSharp.Core.Exceptions;
using EdiSharp.Core.Models.IntermediateModel;

namespace EdiSharp.Core.Extensions;
public static class EdiSegmentExtensions
{
    public static string? GetComponent(this EdiSegment seg, int elemIdx, int compIdx)
    {
        if (seg.Elements.Count <= elemIdx)
            return null;

        if (seg.Elements[elemIdx].Components.Count <= compIdx)
            return null;

        return seg.Elements[elemIdx].Components[compIdx];
    }

    public static string GetRequiredComponent(this EdiSegment seg, int elemIdx, int compIdx)
    {
        return seg.GetComponent(elemIdx, compIdx)
            ?? throw new EdiSemanticsException($"Missing component [{elemIdx}:{compIdx}] in segment {seg.Tag}");
    }
}

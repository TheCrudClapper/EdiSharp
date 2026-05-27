using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;

namespace EdiSharp.Core.Factories.Abstractions;

public interface IEdiMessageSplitterFactory
{
    IEdiMessageSplitter? TryCreate(InputType inputType);
}

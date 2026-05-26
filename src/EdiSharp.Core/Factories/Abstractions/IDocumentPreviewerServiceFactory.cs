using EdiSharp.Core.Enums;
using EdiSharp.Core.ServiceContracts;

namespace EdiSharp.Core.Factories.Abstractions;

public interface IDocumentPreviewerServiceFactory
{
    IDocumentPreviewerService? TryCreate(InputType input);
}
